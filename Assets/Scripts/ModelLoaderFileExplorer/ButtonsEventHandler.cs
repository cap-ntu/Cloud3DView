using UnityEngine;
using System.Collections;

namespace ModelLoaderFileExplorer {

	public class ButtonsEventHandler : MonoBehaviour {

		public ModelImportManager manager;
		public string importModelButtonName;
		public string closeWindowButtonName;
		public string chooseButtonName;
		public string submitButtonName;
		public string cancelSubmitButtonName;


		public void Start () {
			if (importModelButtonName.Equals("") ||
			    closeWindowButtonName.Equals("") ||
			    chooseButtonName.Equals("") ||
			    submitButtonName.Equals("") ||
			    cancelSubmitButtonName.Equals("")) {
				Debug.LogError("Buttons Event Handler: button(s) name not set");
			}
		}

		public void TriggerEvent (GameObject go) {
			if (go.name.Equals(importModelButtonName)) {
				manager.ChangeState(Constants.SELECT_STATE);
			}
			else if (go.name.Equals(closeWindowButtonName)) {
				manager.ChangeState(Constants.IDLE_STATE);
			}
			else if (go.name.Equals(chooseButtonName)) {
				manager.ChangeState(Constants.VIEW_STATE);
			}
			else if (go.name.Equals(submitButtonName)) {
				manager.ChangeState(Constants.UPLOAD_STATE);
			}
			else if (go.name.Equals(cancelSubmitButtonName)) {
				Debug.Log("Here");
				manager.ChangeState(Constants.SELECT_STATE);
			}
		}
	}

}
