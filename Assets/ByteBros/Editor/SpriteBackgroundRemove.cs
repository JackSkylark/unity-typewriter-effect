using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteBackgroundRemove : EditorWindow
{
    Texture2D _img;
    Texture2D _newImg;
    Color _colorToRemove = Color.magenta;
    public static SpriteBackgroundRemove Win;

    [MenuItem("Window/Tools/Alpha-fy Images")]
    static void Init()
    {
        Win = ScriptableObject.CreateInstance(typeof(SpriteBackgroundRemove)) as SpriteBackgroundRemove;
        Win.minSize = new Vector2(300, 350);
        Win.ShowUtility();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        /** Toolbar **/
        GUILayout.BeginVertical();
        _img = (Texture2D)EditorGUILayout.ObjectField(_img, typeof(Texture2D), false, GUILayout.MinWidth(128), GUILayout.MinHeight(128), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128));

        _colorToRemove = EditorGUILayout.ColorField(_colorToRemove, GUILayout.MaxWidth(128));

        if (GUILayout.Button("Preview", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            _newImg = RemoveColor(_colorToRemove, _img);

        if (GUILayout.Button("Alpha-fy All", GUILayout.MinWidth(128), GUILayout.MinHeight(32), GUILayout.MaxWidth(128), GUILayout.MaxHeight(128)))
            RemoveColor(_colorToRemove, (UnityEngine.Object[])Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets));

        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Selected Files", EditorStyles.boldLabel);
        foreach (var selected in Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets))
        {
            GUILayout.Label(selected.name);
        }
        GUILayout.EndVertical();

        /** Image Display **/
        GUILayout.BeginVertical();
        GUILayout.Label("Preview", EditorStyles.boldLabel);
        if (_newImg)
        {
            GUILayout.Label(_newImg);
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

    }

    // for multiple images
    void RemoveColor(Color c, UnityEngine.Object[] imgs)
    {
        if (!Directory.Exists("Assets/AlphaImages/"))
        {
            Directory.CreateDirectory("Assets/AlphaImages/");
        }

        float inc = 0f;
        foreach (Texture2D i in imgs)
        {
            inc++;
            if (inc % 512 == 0 && EditorUtility.DisplayCancelableProgressBar("Playin' With Pixels", "Seaching for Color Matches", ((float)inc / (float)imgs.Length)))
            {
                Debug.LogError("Cancelled..");
                break;
            }

            CheckTextureSettings(i);

            Color[] pixels = i.GetPixels(0, 0, i.width, i.height, 0);
            var clear = new Color(0, 0, 0, 0);

            for (int p = 0; p < pixels.Length; p++)
            {
                if (pixels[p] == c)
                {
                    pixels[p] = clear;
                }
            }

            Texture2D n = new Texture2D(i.width, i.height);
            n.SetPixels(0, 0, i.width, i.height, pixels, 0);
            n.Apply();

            byte[] bytes = n.EncodeToPNG();
            File.WriteAllBytes("Assets/AlphaImages/" + i.name + "_alpha.png", bytes);
        }

        EditorUtility.ClearProgressBar();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // for single image
    Texture2D RemoveColor(Color c, Texture2D i)
    {
        CheckTextureSettings(i);

        Color[] pixels = i.GetPixels(0, 0, i.width, i.height, 0);

        var clear = new Color(0, 0, 0, 0);

        for (int p = 0; p < pixels.Length; p++)
        {
            if (p % 512 == 0 && EditorUtility.DisplayCancelableProgressBar("Playin' With Pixels", "Seaching for Color Matches", ((float)p / pixels.Length)))
            {
                Debug.LogError("Cancelled..");
                break;
            }

            if (pixels[p] == c)
            {
                pixels[p] = clear;
            }

        }

        Texture2D n = new Texture2D(i.width, i.height);
        n.SetPixels(0, 0, i.width, i.height, pixels, 0);
        n.Apply();
        EditorUtility.ClearProgressBar();
        return (n);
    }

    public void CheckTextureSettings(Texture2D texture)
    {
        if (texture == null) { Debug.LogError("CheckTextureSettings Failed - Texture is null"); return; }

        string path = AssetDatabase.GetAssetPath(texture);
        if (string.IsNullOrEmpty(path)) { Debug.LogError("CheckTextureSettings Failed - Texture path is null"); return; }

        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        if (!textureImporter.isReadable)
        {
            Debug.Log("Enabling read/write for image " + path);
            //            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

    }
}
