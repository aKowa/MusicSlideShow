using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(MusicShowManager))]
public class MusicShowManagerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		var musicShowManager = target as MusicShowManager;

		if (GUILayout.Button("Next Music Show"))
		{
			if (musicShowManager != null)
			{
				musicShowManager.SetNextMusicShow();
			}
		}
	}
}
