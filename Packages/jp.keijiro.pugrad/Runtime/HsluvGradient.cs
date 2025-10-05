using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

namespace Pugrad {

// HSLuv gradient functions
public static class HsluvGradient
{
    public static Color Evaluate(float hue, float lightness)
    {
        var hsluv = math.float3(hue * math.PI * 2, 100, lightness * 100);
        Hsluv.ToRgb(hsluv, out float3 rgb);
        return new Color(rgb.x, rgb.y, rgb.z);
    }
}

// HSLuv colorspace converter
//
// Based on the hsluv-csharp implementation, heavily optimized with Burst.
// https://github.com/hsluv/hsluv-csharp
[BurstCompile]
public static class Hsluv
{
    #region Public method

    public static Color ToRgb(float hue, float saturation, float lightness)
    {
        var hsl = math.float3(hue, saturation, lightness);
        ToRgb(hsl, out float3 rgb);
        return new Color(rgb.x, rgb.y, rgb.z);
    }

    public static Color ToRgb(float3 hsluv)
      => ToRgb(hsluv.x, hsluv.y, hsluv.z);

    public static Color ToRgb(Vector3 hsluv)
      => ToRgb(hsluv.x, hsluv.y, hsluv.z);

    [BurstCompile]
    public static void ToRgb(in float3 hsl, out float3 rgb)
      => rgb = XyzToRgb(LuvToXyz(LchToLuv(HsluvToLch(hsl))));

    #endregion

    #region Private implementation

    static readonly float3x3 M = math.float3x3
      ( 3.240969941904521f, -1.537383177570093f,   -0.498610760293f,
        -0.96924363628087f,   1.87596750150772f, 0.041555057407175f,
        0.055630079696993f,  -0.20397695888897f, 1.056971514242878f);

    static readonly float2 RefUV =
      math.float2(0.19783000664283f, 0.46831999493879f);

    // CIE LUV constants
    const float Kappa = 903.2962962f;
    const float Epsilon = 0.0088564516f;

    static float MaxChromaForLH(float2 lh)
    {
        var sub1 = math.pow(lh.x + 16, 3) / 1560896;
        var sub2 = sub1 > Epsilon ? sub1 : lh.x / Kappa;

        var top1 = sub2 * math.mul(M, math.float3(284517, 0, -94839));
        var top2 = sub2 * math.mul(M, math.float3(731718, 769860, 838422));
        var bottom = sub2 * math.mul(M, math.float3(0, -126452, 632260));

        var min = math.float3(System.Single.MaxValue);

        for (var t = 0; t < 2; ++t)
        {
            var div = math.rcp(bottom + 126452 * t);
            var slope = div * top1;
            var inter = div * lh.x * (top2 - 769860 * t);
            var length = inter / (math.sin(lh.y) - slope * math.cos(lh.y));
            min = math.select(min, length, length < min & length >= 0);
        }

        return math.cmin(min);
    }

    static float3 FromLinear(float3 c)
      => math.select
         (12.92f * c, 1.055f * math.pow(c, 1 / 2.4f) - 0.055f, c > 0.0031308f);

    static float3 XyzToRgb(float3 xyz)
      => FromLinear(math.mul(M, xyz));

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

    #endregion
}

} // namespace Pugrad
