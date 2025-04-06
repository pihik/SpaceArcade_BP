using UnityEngine;
using UnityEditor;
using System.IO;

public class LineCounter
{
	[MenuItem("Tools/Count C# Lines")]
	static void CountLines()
	{
		string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
		int totalLines = 0;

		foreach (string file in files)
		{
			totalLines += File.ReadAllLines(file).Length;
		}

		Debug.Log($"Total C# Lines: {totalLines}");
	}
}
