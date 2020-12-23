using UnityEngine;

namespace Pugrad {

static class HsluvGradient
{
    public static Color [] Generate(uint resolution, float lightness)
    {
        var pixels = new Color[resolution];

        var hsluv = new double [] { 0, 100, lightness * 100 };

        for (var i = 0; i < resolution; i++)
        {
            hsluv[0] = i * 360.0 / resolution;
            var rgb = Hsluv.HsluvConverter.HsluvToRgb(hsluv);
            pixels[i] = new Color((float)rgb[0], (float)rgb[1], (float)rgb[2]);
        }

        return pixels;
    }
}

} // namespace Pugrad
