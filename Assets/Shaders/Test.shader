Shader "Pugrad/Test"
{
    CGINCLUDE

#include "UnityCG.cginc"
#include "Packages/jp.keijiro.pugrad/Shaders/HsluvColormap.hlsl"
#include "Packages/jp.keijiro.pugrad/Shaders/MatplotlibColormaps.hlsl"
#include "Packages/jp.keijiro.pugrad/Shaders/TurboColormap.hlsl"

void Vertex(float4 position : POSITION,
            float2 texcoord : TEXCOORD0,
            out float4 outPosition : SV_Position,
            out float2 outTexcoord : TEXCOORD0)
{
    outPosition = UnityObjectToClipPos(position);
    outTexcoord = texcoord;
}

float4 Fragment(float4 cs : SV_Position, float2 uv : TEXCOORD0) : SV_Target
{
    float3 rgb;

    if (uv.y < 1.0 / 6)
    {
        rgb = PugradHsluv(float3(uv.x * UNITY_PI * 2, 100, 60));
    }
    else if (uv.y < 2.0 / 6)
    {
        rgb = PugradTurbo(uv.x);
    }
    else if (uv.y < 3.0 / 6)
    {
        rgb = PugradInferno(uv.x);
    }
    else if (uv.y < 4.0 / 6)
    {
        rgb = PugradMagma(uv.x);
    }
    else if (uv.y < 5.0 / 6)
    {
        rgb = PugradPlasma(uv.x);
    }
    else
    {
        rgb = PugradViridis(uv.x);
    }

    return float4(GammaToLinearSpace(rgb), 1);
}

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
