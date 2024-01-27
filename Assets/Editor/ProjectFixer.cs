using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ProjectFixer : Editor
{
	[MenuItem("Project Fixer/Sort Animations")]
	public static void FixAnims()
	{
		if (!EditorUtility.DisplayDialog("Are you sure?", "Make sure you have backups!! this will also rename files with AR or DevX's naming conventions that are MEANT to have that name!", "Proceed", "Cancel"))
		{
			return;
		}

		string split = (EditorUtility.DisplayDialog("Naming convention", "Are you renaming AssetRipper or DevX animations?", "Assetripper", "DevX")  ? "_" : "_d");

		foreach (string file in Directory.GetFiles(Application.dataPath + "/AnimationClip"))
		{
			string extension = Path.GetExtension(file);

			if (extension == ".meta")
			{
				continue;
			}

			string[] splitName = Path.GetFileNameWithoutExtension(file).Split(new string[]{split}, System.StringSplitOptions.None);

			try
			{
				int.Parse(splitName[splitName.Length - 1]);
			}
			catch
			{
				continue;
			}
			if (splitName.Length == 1)
			{
				continue;
			}

			string path = Path.GetDirectoryName(file) + "/" + splitName[splitName.Length - 1].Replace(".anim", "");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			File.Move(file, path + "/" + Path.GetFileName(file).Replace(split + splitName[splitName.Length - 1], ""));
			File.Move(file + ".meta", path + "/" + Path.GetFileName(file).Replace(split + splitName[splitName.Length - 1], "") + ".meta");
		}

		AssetDatabase.Refresh();
	}

	[MenuItem("Project Fixer/Sort Lightmaps")]
	public static void SortLightmaps()
	{
		string[] paths = (from directory in Directory.GetFiles(Application.dataPath + "/Texture2D/") where directory.Contains("LightmapFar") select directory).ToArray();

		Debug.LogError(paths.Length);

		foreach (string path in paths)
		{
			SortLightmap(path);
		}

		AssetDatabase.Refresh();
	}

	private static void SortLightmap(string path)
	{
		if (path.Contains("meta"))
		{
			return;
		}

		string fileName = Path.GetFileNameWithoutExtension(path);

		int index = int.Parse(fileName.Replace("LightmapFar-", "").Split('_')[0]);
		int group = fileName.Contains("_") ? int.Parse(fileName.Split('_')[1]) + 1 : 0;

		if (!Directory.Exists(Application.dataPath + "/Texture2D/Lightmap_" + group))
		{
			Directory.CreateDirectory(Application.dataPath + "/Texture2D/Lightmap_" + group);
		}

		File.Move(path, Application.dataPath + "/Texture2D/Lightmap_" + group + "/LightmapFar_" + index + ".png");
		File.Move(Path.GetDirectoryName(path) + "/" + fileName + ".png.meta", Application.dataPath + "/Texture2D/Lightmap_" + group + "/LightmapFar_" + index + ".png.meta");
	}
}
