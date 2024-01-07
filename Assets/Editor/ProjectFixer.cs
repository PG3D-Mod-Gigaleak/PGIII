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
}
