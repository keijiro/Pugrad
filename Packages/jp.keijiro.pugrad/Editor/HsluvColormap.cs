using UnityEngine;
using Unity.Mathematics;

namespace Pugrad {

// HSLuv colormap generator
static class HsluvColormap
{
    public static Color[] Generate(uint resolution, float lightness)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = HsluvGradient.Evaluate((float)i / resolution, lightness);
        return pixels;
    }
}

} // namespace Pugrad
