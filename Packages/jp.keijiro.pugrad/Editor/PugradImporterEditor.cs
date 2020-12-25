using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Pugrad {

[CustomEditor(typeof(PugradImporter)), CanEditMultipleObjects]
sealed class PugradImporterEditor : ScriptedImporterEditor
{
    SerializedProperty _colormap;
    SerializedProperty _resolution;
    SerializedProperty _lightness;

    static readonly GUIContent[] ColormapLabels =
      Enum.GetNames(typeof(ColormapType)).
      Select(x => new GUIContent(x)).ToArray();

    static readonly int[] ColormapValues =
      Enumerable.Range(0, Enum.GetValues(typeof(ColormapType)).Length).
      ToArray();

    public override void OnEnable()
    {
        base.OnEnable();
        _colormap   = serializedObject.FindProperty("_colormap");
        _resolution = serializedObject.FindProperty("_resolution");
        _lightness  = serializedObject.FindProperty("_lightness");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.IntPopup(_colormap, ColormapLabels, ColormapValues);
        EditorGUILayout.PropertyField(_resolution);

        if (_colormap.hasMultipleDifferentValues ||
            (ColormapType)_colormap.enumValueIndex == ColormapType.HSLuv)
          EditorGUILayout.PropertyField(_lightness);

        serializedObject.ApplyModifiedProperties();
        ApplyRevertGUI();
    }

    [MenuItem("Assets/Create/Pugrad")]
    public static void CreateNewAsset()
      => ProjectWindowUtil.CreateAssetWithContent("New Colormap.pugrad", "");
}

} // namespace Pugrad
