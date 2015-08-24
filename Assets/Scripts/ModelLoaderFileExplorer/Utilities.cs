using UnityEngine;
using System.Collections;
using System.IO;
using RuntimeModelLoader.Ex;
using RMI;

namespace ModelLoaderFileExplorer {

	public class Utilities {

		public static bool CheckSupportedFileFormat (string filePath) {
			string ext = Path.GetExtension(filePath);
			
			// Add supported file format here.
			switch (ext) {
			case ".obj":
				return true;
			case ".fbx":
				return true;
			}
//			if (ext.Equals(".obj")) {
//				return true;
//			}
//			
			return false;
		}
		
		public static GameObject LoadModelFile (string filePath, Material material) {
			GameObject modelGo = ModelLoaderEx.LoadModelGameObject(filePath, material);
			modelGo.name = Path.GetFileName(filePath);
			return modelGo;
		}

		public static GameObject LoadModelFileRMI (string filePath, RMITask task, RMITask.RMIEndCallback callback) {
			task.Load (filePath, Vector3.zero, Quaternion.identity, callback);
			return null;
		}

		public static void SetLayerRecursively (GameObject go, int layerNumber) {
			go.layer = layerNumber;
			Transform trans = go.transform;

			foreach (Transform childTrans in trans) {
				childTrans.gameObject.layer = layerNumber;
				SetLayerRecursively(childTrans.gameObject, layerNumber);
			}
		}

		public static void NormalizeMeshSize (GameObject go) {
			MeshRenderer[] renderers = go.transform.GetComponentsInChildren<MeshRenderer>(true);
			float scale = 1;
//			foreach (MeshRenderer renderer in renderers) {
//				Vector3 size = renderer.bounds.size;
//				size *= scale;
//
//				while (size.x >= 10 || size.y >= 10 || size.z >= 10) {
//					scale /= 10;
//					size /= 10;
//				}
//			}

			Transform[] transforms = go.GetComponentsInChildren<Transform>(true);
			foreach (Transform trans in transforms) {
				Vector3 localScale = trans.localScale;
				localScale *= scale;

				while (localScale.x >= 1 || localScale.y >= 1 || localScale.z >= 1) {
					scale /= 10;
					localScale /= 10;
				}
			}

			go.transform.localScale *= scale;
		}

		public static string GetPersistancePath () {
			var path = Application.persistentDataPath;
			if (Application.platform == RuntimePlatform.WindowsEditor ||
			    Application.platform == RuntimePlatform.WindowsPlayer ||
			    Application.platform == RuntimePlatform.WindowsWebPlayer) {
				path = path.Replace('/', '\\');
			}
			return path;
		}
	}

}