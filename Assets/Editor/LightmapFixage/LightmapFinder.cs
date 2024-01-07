using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightmapFinder : EditorWindow
{
    [MenuItem("Lightmaps/Open Lightmap Fixer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LightmapFinder));
    }

	private static List<Material> lightmapDiffuse, lightmapTransparent;

	private static List<Texture2D> lightmaps;

	private int lightmapIndex;

    
    void OnGUI()
    {
		GUILayout.Label("Lightmap Fixage", EditorStyles.largeLabel);

		if (GUILayout.Button("Refresh lightmaps", GUILayout.Height(45), GUILayout.Width(125)))
		{
			lightmapDiffuse = LightmapUtility.DiffuseShadersInThisScene;
			lightmapTransparent = LightmapUtility.TransparentShadersInThisScene;
			lightmaps = LightmapUtility.LightMapTextures;
			lightmapIndex = 0;
			LightmapUtility.SetDiffuseShaders(lightmapDiffuse);
			LightmapUtility.SetTransparentShaders(lightmapTransparent);
			LightmapUtility.ChangeLightmaps(lightmapDiffuse, lightmaps[lightmapIndex]);
			LightmapUtility.ChangeLightmaps(lightmapTransparent, lightmaps[lightmapIndex]);
		}

		if (lightmaps == null)
		{
			Debug.LogError("REFRESH THE LIGHTMAPS!!! DO IT NOW!!! DO IT!!! DO IT NOW!!!");
			return;
		}
		
		if (GUILayout.Button("Next", GUILayout.Height(35), GUILayout.Width(175)))
		{
			if (lightmapIndex < lightmaps.Count - 1)
			{
				lightmapIndex++;
				LightmapUtility.ChangeLightmaps(lightmapDiffuse, lightmaps[lightmapIndex]);
				LightmapUtility.ChangeLightmaps(lightmapTransparent, lightmaps[lightmapIndex]);
			}
		}
		if (GUILayout.Button("Last", GUILayout.Height(35), GUILayout.Width(175)))
		{
			if (lightmapIndex != 0)
			{
				lightmapIndex--;
				LightmapUtility.ChangeLightmaps(lightmapDiffuse, lightmaps[lightmapIndex]);
				LightmapUtility.ChangeLightmaps(lightmapTransparent, lightmaps[lightmapIndex]);
			}
		}

		EditorGUI.DrawPreviewTexture(new Rect(285, 15, 100, 100), lightmaps[lightmapIndex]);
    }
}