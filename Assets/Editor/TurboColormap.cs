using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

namespace Pugrad {

// Google Turbo colormap generator
static class TurboColormap
{
    public static Color[] Generate(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
        {
            TurboHelper.GetColor((float)i / resolution, out float3 rgb);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }
        return pixels;
    }
}

// Internal implementation with burst
//
// Original code:
// https://gist.github.com/mikhailov-work/0d177465a8151eb6ede1768d51d476c7
[BurstCompile]
static class TurboHelper
{
    static readonly float3x4 M1 =
      math.float3x4(0.13572138f,  4.61539260f, -42.66032258f, 132.13108234f,
                    0.09140261f,  2.19418839f,   4.84296658f, -14.18503333f,
                    0.10667330f, 12.64194608f, -60.58204836f, 110.36276771f);

    static readonly float3x2 M2 =
      math.float3x2(-152.94239396f, 59.28637943f,
                       4.27729857f,  2.82956604f,
                     -89.90310912f, 27.34824973f);

    [BurstCompile]
    public static void GetColor(float x, out float3 rgb)
    {
        var v = math.float4(1, x, x * x, x * x * x);
        rgb = math.mul(M1, v) + math.mul(M2, v.zw * v.z);
    }
}

} // namespace Pugrad
