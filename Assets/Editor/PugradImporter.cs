using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Pugrad {

public enum ColorMapType { Turbo, HSLuv }

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

        if (_colorMap == ColorMapType.Turbo)
            texture.SetPixels(TurboGradient.Generate(_resolution));
        else if (_colorMap == ColorMapType.HSLuv)
            texture.SetPixels(HsluvGradient.Generate(_resolution, _lightness));

        texture.Apply();

        context.AddObjectToAsset("texture", texture);
        context.SetMainObject(texture);
    }

    #endregion
}

} // namespace Pugrad
