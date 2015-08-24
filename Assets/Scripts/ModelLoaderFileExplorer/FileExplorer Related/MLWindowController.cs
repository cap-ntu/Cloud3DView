using UnityEngine;
using System.Collections;
using FileExplorerNGUI.Ex;

namespace ModelLoaderFileExplorer {

	public class MLWindowController : WindowControllerNGUI {

		public Vector3 modelPosition {get; set;}


		public override void OnRegistered () {
			window.otherButton.isEnabled = false;
		}

		protected override void OnCancelButtonPressed () {
			base.OnCancelButtonPressed ();

			ButtonsEventHandler handler = GameObject.FindObjectOfType<ButtonsEventHandler>();
			handler.TriggerEvent(UIButton.current.gameObject);
		}

		protected override void OnOtherButtonPressed () {
			if (Utilities.CheckSupportedFileFormat(window.activeFilePath)) {
				GameObject modelGo = Utilities.LoadModelFile(window.activeFilePath, null);
				modelGo.transform.position = modelPosition;

				ModelImportManager.instance.modelGo = modelGo;

				ButtonsEventHandler handler = GameObject.FindObjectOfType<ButtonsEventHandler>();
				handler.TriggerEvent(UIButton.current.gameObject);
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
	}

}