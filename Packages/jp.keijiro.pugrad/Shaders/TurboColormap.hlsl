#ifndef PUGRAD_TURBO_COLORMAP_INCLUDED
#define PUGRAD_TURBO_COLORMAP_INCLUDED

// Google Turbo colormap function
// Original code: https://gist.github.com/mikhailov-work/0d177465a8151eb6ede1768d51d476c7

float3 PugradTurbo(float x)
{
    float4 v = float4(1, x, x * x, x * x * x);

    float3x4 m1 = float3x4(
        0.13572138, 4.61539260, -42.66032258, 132.13108234,
        0.09140261, 2.19418839, 4.84296658, -14.18503333,
        0.10667330, 12.64194608, -60.58204836, 110.36276771
    );

    float3x2 m2 = float3x2(
        -152.94239396, 59.28637943,
        4.27729857, 2.82956604,
        -89.90310912, 27.34824973
    );

    return mul(m1, v) + mul(m2, v.zw * v.z);
}

#endif // PUGRAD_TURBO_COLORMAP_INCLUDED
