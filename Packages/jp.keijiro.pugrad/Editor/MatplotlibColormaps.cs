using UnityEngine;

namespace Pugrad {

// Matplotlib colormap generators
static class MatplotlibColormaps
{
    public static Color[] GenerateViridis(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = ViridisGradient.Evaluate((float)i / resolution);
        return pixels;
    }

    public static Color[] GeneratePlasma(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = PlasmaGradient.Evaluate((float)i / resolution);
        return pixels;
    }

    public static Color[] GenerateMagma(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = MagmaGradient.Evaluate((float)i / resolution);
        return pixels;
    }

    public static Color[] GenerateInferno(uint resolution)
    {
        var pixels = new Color[resolution];
        for (var i = 0; i < resolution; i++)
            pixels[i] = InfernoGradient.Evaluate((float)i / resolution);
        return pixels;
    }
}

} // namespace Pugrad
