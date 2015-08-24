using UnityEngine;
using System.Collections;
using FileExplorerNGUI.Ex;

namespace ModelLoaderFileExplorer {
	
	public class UploadModelControllerComponent : WindowHelperComponent {

		public Vector3 modelPosition;
		public string modelName;
		public Transform modelContainer;
		public RMITask importTask;
		public Material modelMaterial;
		public UIWindowButton windowButton;

		protected override WindowControllerNGUI GetController () {
			var controller = new UploadModelController();
			controller.modelPosition = modelPosition;
			controller.modelName = modelName;
			controller.modelContainer = modelContainer;
			controller.modelMaterial = modelMaterial;
			controller.importTask = importTask;
			controller.windowButton = windowButton;

			return controller;
		}
	}

}