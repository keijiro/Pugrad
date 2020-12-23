using UnityEngine;

sealed class HsluvGradient : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.RawImage _display = null;

    Texture2D _texture;

    void Start()
    {
        var width = 256;

        _texture = new Texture2D(width, 1);

        var hsluv = new double [] { 0, 100, 55 };
        var pixels = new Color[width];

        for (var i = 0; i < width; i++)
        {
            hsluv[0] = (float)i / width * 360;
            var rgb = Hsluv.HsluvConverter.HsluvToRgb(hsluv);
            pixels[i] = new Color((float)rgb[0], (float)rgb[1], (float)rgb[2], 1);
        }

        _texture.SetPixels(pixels);
        _texture.Apply();

        _display.texture = _texture;
    }
}
