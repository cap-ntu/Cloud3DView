using UnityEngine;
using System.Collections;
using FileExplorerNGUI.Ex;
using ModelLoaderFileExplorer;


public class DefaultControllerComponent : WindowHelperComponent {

	protected override WindowControllerNGUI GetController () {
		return new CloseAllController();
	}

	public class CloseAllController : WindowControllerNGUI {
		protected override void OnOtherButtonPressed () {
			base.OnOtherButtonPressed ();

			FileExplorerNGUIEx.hidden = true;
		}

		public override void OnFileHighlighted (string path) {
			if (Utilities.CheckSupportedFileFormat(path)) {
				window.otherButton.isEnabled = true;
			}
			else {
				window.otherButton.isEnabled = false;
			}
		}

		public override void OnRegistered () {
			base.OnRegistered ();

			window.otherButton.isEnabled = false;
		}
	}
}
