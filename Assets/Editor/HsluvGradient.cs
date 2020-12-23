using UnityEngine;
using Unity.Mathematics;

namespace Pugrad {

static class HsluvGradient
{
    public static Color [] Generate(uint resolution, float lightness)
    {
        var pixels = new Color[resolution];

        var hsluv = math.float3(0, 100, lightness * 100);

        for (var i = 0; i < resolution; i++)
        {
            hsluv.x = i * math.PI * 2 / resolution;
            var rgb = HsluvToRgb(hsluv);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }

        return pixels;
    }

    static float3x3 M = math.float3x3
      ( 3.240969941904521f, -1.537383177570093f, -0.498610760293f   ,
       -0.96924363628087f ,  1.87596750150772f ,  0.041555057407175f,
        0.055630079696993f, -0.20397695888897f ,  1.056971514242878f);
    static float2 RefUV = math.float2(0.19783000664283f, 0.46831999493879f);
    static float Kappa   = 903.2962962f;
    static float Epsilon = 0.0088564516f;

    static float LengthOfRayUntilIntersect(float theta, float2 line)
    {
        var length = line.y / (math.sin(theta) - line.x * math.cos(theta));
        return length >= 0 ? length : System.Single.MaxValue;
    }

    static float MaxChromaForLH(float2 lh)
    {
        var min = System.Single.MaxValue;

        var sub1 = math.pow(lh.x + 16, 3) / 1560896;
        var sub2 = sub1 > Epsilon ? sub1 : lh.x / Kappa;

        for (var c = 0; c < 3; ++c)
        {
            var m1 = M[0][c];
            var m2 = M[1][c];
            var m3 = M[2][c];

            for (var t = 0; t < 2; ++t)
            {
                var top1 = (284517 * m1 - 94839 * m3) * sub2;
                var top2 = (838422 * m3 + 769860 * m2 + 731718 * m1) *
                  lh.x * sub2 - 769860 * t * lh.x;
                var bottom = (632260 * m3 - 126452 * m2) * sub2 + 126452 * t;
                var bound = math.float2(top1, top2) / bottom;
                min = math.min(min, LengthOfRayUntilIntersect(lh.y, bound));
            }
        }

        return min;
    }

    static float3 XyzToRgb(float3 xyz)
      => math.mul(M, xyz);

    static float LToY(float L) 
      => L <= 8 ? L / Kappa : math.pow((L + 16) / 116, 3);

    static float3 LuvToXyz(float3 luv)
    {
        if (luv.x == 0) return float3.zero;
        var UV = luv.yz / (13 * luv.x) + RefUV;
        var Y = LToY(luv.x);
        var X = 9 * Y * UV.x / (4 * UV.y);
        var Z = (9 * Y - (X + 15 * Y) * UV.y) / (3 * UV.y);
        return math.float3(X, Y, Z);
    }

    static float3 LchToLuv(float3 lch)
      => lch.xyy * math.float3(1, math.cos(lch.z), math.sin(lch.z));

    static float3 HsluvToLch(float3 hsl)
      => hsl.zyx * math.float3(1, MaxChromaForLH(hsl.zx) / 100, 1);

    public static float3 HsluvToRgb(float3 hsl)
      => XyzToRgb(LuvToXyz(LchToLuv(HsluvToLch(hsl))));
}

} // namespace Pugrad
