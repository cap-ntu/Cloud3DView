using UnityEngine;
using System.Collections;
using FileExplorerNGUI.Ex;

namespace ModelLoaderFileExplorer {
	
	public class UploadModelController : WindowControllerNGUI {
		
		public Vector3 modelPosition {get; set;}
		public string modelName {get; set;}
		public Transform modelContainer;
		public RMITask importTask;
		public Material modelMaterial;
		public UIWindowButton windowButton;

		public static string previousLoadedModelPath;
		
		
		public override void OnRegistered () {
			window.otherButton.isEnabled = false;
		}
		
		protected override void OnCancelButtonPressed () {
			base.OnCancelButtonPressed ();

			// TODO: open the main menu window
		}
		
		protected override void OnOtherButtonPressed () {
			if (Utilities.CheckSupportedFileFormat(window.activeFilePath)) {
				var task = (RMITask) GameObject.Instantiate(importTask);
				Utilities.LoadModelFileRMI(window.activeFilePath, task, EndCallBack);
//				EndCallBackForTest();

				// TODO: open the view window
			}
		}
		
		public override void OnFileHighlighted (string path) {
			if (Utilities.CheckSupportedFileFormat(path)) {
				window.otherButton.isEnabled = true;
			}
			else {
				window.otherButton.isEnabled = false;
			}
		}

		private void EndCallBack (RMI.RMITask inTask) {
			GameObject modelGo = inTask.ImportedObject;
			Utilities.SetLayerRecursively(modelGo, LayerMask.NameToLayer("UI 3D"));
			modelGo.transform.parent = modelContainer;
			modelGo.transform.localPosition = modelPosition;
			modelGo.name = modelName;
			Utilities.NormalizeMeshSize(modelGo);

			previousLoadedModelPath = inTask.FilePath;

			windowButton.action = UIWindowButton.Action.Show;
			windowButton.WindowAction();
			windowButton.action = UIWindowButton.Action.DoNothing;
		}

		private void EndCallBackForTest () {
			GameObject modelGo = new GameObject();
			Utilities.SetLayerRecursively(modelGo, LayerMask.NameToLayer("UI 3D"));
			modelGo.transform.parent = modelContainer;
			modelGo.transform.localPosition = modelPosition;
			modelGo.name = modelName;
			
			previousLoadedModelPath = window.activeFilePath;
			
			windowButton.action = UIWindowButton.Action.Show;
			windowButton.WindowAction();
			windowButton.action = UIWindowButton.Action.DoNothing;
		}
	}
	
}