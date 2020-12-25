using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;

namespace Pugrad {

// Matplotlib colormap generators
static class MatplotlibColormaps
{
    public static Color[] GenerateViridis(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
        {
            MatplotlibHelper.GetViridisColor((float)i / resolution, out float3 rgb);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }
        return pixels;
    }

    public static Color[] GeneratePlasma(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
        {
            MatplotlibHelper.GetPlasmaColor((float)i / resolution, out float3 rgb);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }
        return pixels;
    }

    public static Color[] GenerateMagma(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
        {
            MatplotlibHelper.GetMagmaColor((float)i / resolution, out float3 rgb);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }
        return pixels;
    }

    public static Color[] GenerateInferno(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
        {
            MatplotlibHelper.GetInfernoColor((float)i / resolution, out float3 rgb);
            pixels[i] = new Color(rgb.x, rgb.y, rgb.z);
        }
        return pixels;
    }
}

// Internal implementation with burst
//
// Original code:
// https://www.shadertoy.com/view/WlfXRN
[BurstCompile]
static class MatplotlibHelper
{
    static readonly float3x4 Viridis1 = math.float3x4
      (0.2777273272234177f, 0.1050930431085774f, -0.3308618287255563f, -4.634230498983486f,
       0.005407344544966578f, 1.404613529898575f, 0.214847559468213f, -5.799100973351585f,
       0.3340998053353061f, 1.384590162594685f, 0.09509516302823659f, -19.33244095627987f);

    static readonly float3x3 Viridis2 = math.float3x3
      (6.228269936347081f, 4.776384997670288f, -5.435455855934631f,
       14.17993336680509f, -13.74514537774601f, 4.645852612178535f,
       56.69055260068105f, -65.35303263337234f, 26.3124352495832f);

    [BurstCompile]
    public static void GetViridisColor(float x, out float3 rgb)
    {
        var v = math.float4(1, x, x * x, x * x * x);
        rgb = math.mul(Viridis1, v) + math.mul(Viridis2, v.yzw * v.w);
    }

    static readonly float3x4 Plasma1 = math.float3x4
      (0.05873234392399702f, 2.176514634195958f, -2.689460476458034f, 6.130348345893603f,
       0.02333670892565664f, 0.2383834171260182f, -7.455851135738909f, 42.3461881477227f,
       0.5433401826748754f, 0.7539604599784036f, 3.110799939717086f, -28.51885465332158f);
      
    static readonly float3x3 Plasma2 = math.float3x3
      (-11.10743619062271f, 10.02306557647065f, -3.658713842777788f,
       -82.66631109428045f, 71.41361770095349f, -22.93153465461149f,
       60.13984767418263f, -54.07218655560067f, 18.19190778539828f);
      
    [BurstCompile]
    public static void GetPlasmaColor(float x, out float3 rgb)
    {
        var v = math.float4(1, x, x * x, x * x * x);
        rgb = math.mul(Plasma1, v) + math.mul(Plasma2, v.yzw * v.w);
    }

    static readonly float3x4 Magma1 = math.float3x4
      (-0.002136485053939582f, 0.2516605407371642f, 8.353717279216625f, -27.66873308576866f,
       -0.000749655052795221f, 0.6775232436837668f, -3.577719514958484f, 14.26473078096533f,
       -0.005386127855323933f, 2.494026599312351f, 0.3144679030132573f, -13.64921318813922f);
      
    static readonly float3x3 Magma2 = math.float3x3
      (52.17613981234068f, -50.76852536473588f, 18.65570506591883f,
       -27.94360607168351f, 29.04658282127291f, -11.48977351997711f,
       12.94416944238394f, 4.23415299384598f, -5.601961508734096f);
      
    [BurstCompile]
    public static void GetMagmaColor(float x, out float3 rgb)
    {
        var v = math.float4(1, x, x * x, x * x * x);
        rgb = math.mul(Magma1, v) + math.mul(Magma2, v.yzw * v.w);
    }

    static readonly float3x4 Inferno1 = math.float3x4
      (0.0002189403691192265f, 0.1065134194856116f, 11.60249308247187f, -41.70399613139459f,
       0.001651004631001012f, 0.5639564367884091f, -3.972853965665698f, 17.43639888205313f,
       -0.01948089843709184f, 3.932712388889277f, -15.9423941062914f, 44.35414519872813f);
      
    static readonly float3x3 Inferno2 = math.float3x3
      (77.162935699427f, -71.31942824499214f, 25.13112622477341f,
       -33.40235894210092f, 32.62606426397723f, -12.24266895238567f,
       -81.80730925738993f, 73.20951985803202f, -23.07032500287172f);

    [BurstCompile]
    public static void GetInfernoColor(float x, out float3 rgb)
    {
        var v = math.float4(1, x, x * x, x * x * x);
        rgb = math.mul(Inferno1, v) + math.mul(Inferno2, v.yzw * v.w);
    }
}

} // namespace Pugrad
