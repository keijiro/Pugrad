using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Pugrad {

public enum ColorMapType { Viridis, Plasma, Magma, Inferno, Turbo, HSLuv }

[ScriptedImporter(1, "pugrad")]
public sealed class PugradImporter : ScriptedImporter
{
    #region ScriptedImporter implementation

    [SerializeField] ColorMapType _colorMap = ColorMapType.HSLuv;
    [SerializeField] uint _resolution = 256;
    [SerializeField] float _lightness = 0.5f;

    public override void OnImportAsset(AssetImportContext context)
    {
        var texture = new Texture2D((int)_resolution, 1);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(GetPixels(_colorMap, _resolution, _lightness));
        texture.Apply();

        context.AddObjectToAsset("texture", texture);
        context.SetMainObject(texture);
    }

    static Color [] GetPixels(ColorMapType type, uint width, float lightness)
      => type switch {
        ColorMapType.Viridis => MatplotlibColormaps.GenerateViridis(width),
        ColorMapType.Plasma  => MatplotlibColormaps.GeneratePlasma(width),
        ColorMapType.Magma   => MatplotlibColormaps.GenerateMagma(width),
        ColorMapType.Inferno => MatplotlibColormaps.GenerateInferno(width),
        ColorMapType.Turbo   => TurboGradient.Generate(width),
        ColorMapType.HSLuv   => HsluvGradient.Generate(width, lightness),
        _ => null
      };

    #endregion
}

} // namespace Pugrad
