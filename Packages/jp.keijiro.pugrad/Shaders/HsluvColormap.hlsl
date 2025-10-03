#ifndef PUGRAD_HSLUV_COLORMAP_INCLUDED
#define PUGRAD_HSLUV_COLORMAP_INCLUDED

// HSLuv colorspace converter
// Based on hsluv-csharp implementation: https://github.com/hsluv/hsluv-csharp

static const float3x3 PUGRAD_HSLUV_M = float3x3(
    3.240969941904521, -1.537383177570093, -0.498610760293,
    -0.96924363628087, 1.87596750150772, 0.041555057407175,
    0.055630079696993, -0.20397695888897, 1.056971514242878
);

static const float2 PUGRAD_HSLUV_REF_UV = float2(0.19783000664283, 0.46831999493879);
static const float PUGRAD_HSLUV_KAPPA = 903.2962962;
static const float PUGRAD_HSLUV_EPSILON = 0.0088564516;

float PugradHsluvMaxChromaForLH(float2 lh)
{
    float sub1 = pow(lh.x + 16, 3) / 1560896;
    float sub2 = sub1 > PUGRAD_HSLUV_EPSILON ? sub1 : lh.x / PUGRAD_HSLUV_KAPPA;

    float3 top1 = sub2 * mul(PUGRAD_HSLUV_M, float3(284517, 0, -94839));
    float3 top2 = sub2 * mul(PUGRAD_HSLUV_M, float3(731718, 769860, 838422));
    float3 bottom = sub2 * mul(PUGRAD_HSLUV_M, float3(0, -126452, 632260));

    float3 minLength = float3(1e10, 1e10, 1e10);

    for (int t = 0; t < 2; ++t)
    {
        float3 div = 1.0 / (bottom + 126452 * t);
        float3 slope = div * top1;
        float3 inter = div * lh.x * (top2 - 769860 * t);
        float3 length = inter / (sin(lh.y) - slope * cos(lh.y));
        minLength = min(minLength, length >= 0 ? length : float3(1e10, 1e10, 1e10));
    }

    return min(minLength.x, min(minLength.y, minLength.z));
}

float3 PugradHsluvFromLinear(float3 c)
{
    return c > 0.0031308 ? 1.055 * pow(c, 1.0 / 2.4) - 0.055 : 12.92 * c;
}

float3 PugradHsluvXyzToRgb(float3 xyz)
{
    return PugradHsluvFromLinear(mul(PUGRAD_HSLUV_M, xyz));
}

float PugradHsluvLToY(float L)
{
    return L <= 8 ? L / PUGRAD_HSLUV_KAPPA : pow((L + 16) / 116, 3);
}

float3 PugradHsluvLuvToXyz(float3 luv)
{
    if (luv.x == 0)
        return float3(0, 0, 0);

    float2 uv = luv.yz / (13 * luv.x) + PUGRAD_HSLUV_REF_UV;
    float y = PugradHsluvLToY(luv.x);
    float x = 9 * y * uv.x / (4 * uv.y);
    float z = (9 * y - (x + 15 * y) * uv.y) / (3 * uv.y);

    return float3(x, y, z);
}

float3 PugradHsluvLchToLuv(float3 lch)
{
    return lch.xyy * float3(1, cos(lch.z), sin(lch.z));
}

float3 PugradHsluvHsluvToLch(float3 hsl)
{
    return hsl.zyx * float3(1, PugradHsluvMaxChromaForLH(hsl.zx) / 100, 1);
}

float3 PugradHsluv(float3 hsl)
{
    return PugradHsluvXyzToRgb(
        PugradHsluvLuvToXyz(
            PugradHsluvLchToLuv(
                PugradHsluvHsluvToLch(hsl)
            )
        )
    );
}

#endif // PUGRAD_HSLUV_COLORMAP_INCLUDED
