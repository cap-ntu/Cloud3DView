//----------------------------------------------
// © 2013 Tinbabu
//----------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using RMI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RMIManagerSample : RMIManagerHelper
{	
	public RMITask taskPrefab;
	public string importFilePath = "Enter the path";

	void OnGUI ()
	{
		if (GUILayout.Button ("Load", GUILayout.Width(80)))
		{
			#if UNITY_EDITOR
			importFilePath = EditorUtility.OpenFilePanel (
				"",
				importFilePath,
				"*.*");
			#endif

			if (importFilePath.Length != 0)
			{
				RMITask rMITask = Instantiate (taskPrefab) as RMITask;

				//rMITask.Load (importFilePath);
				rMITask.meshSetting.isReadMesh = true;
				rMITask.meshSetting.isReadMaterials = true;
				rMITask.Load (importFilePath, Vector3.zero, Quaternion.identity, EndCallBack);

			}
		}
	}

	void EndCallBack (RMI.RMITask inTask)
	{
		if (inTask.ImportedObject)
		{
			Debug.Log ("Success " + inTask.ImportedObject.name);

			inTask.ImportedObject.SetActive (true);

			if (inTask.ImportedObject.animation)
				inTask.ImportedObject.animation.Play ();
		}
		else
			Debug.LogError ("Failure " + inTask.FilePath);

		Destroy (inTask.gameObject);
	}
}



