using UnityEngine;

namespace Pugrad {

// Google Turbo colormap generator
static class TurboColormap
{
    public static Color[] Generate(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = TurboGradient.Evaluate((float)i / resolution);
        return pixels;
    }
}

} // namespace Pugrad
