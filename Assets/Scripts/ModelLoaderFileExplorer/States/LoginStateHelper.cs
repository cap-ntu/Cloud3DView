using UnityEngine;
using System.Collections;

namespace ModelLoaderFileExplorer {

	public class LoginStateHelper : MonoBehaviour {
		public UIInput inputUsername;
		public UIInput inputPassword;
		public UIButton loginButton;
		public UILabel infoLabel;

		public void ToggleInputs (bool isEnabled) {
			inputUsername.collider.enabled = isEnabled;
			inputPassword.collider.enabled = isEnabled;

			if (isEnabled) {
				loginButton.state = UIButton.State.Normal;
			}
			else {
				loginButton.state = UIButton.State.Disabled;
			}
		}

		public void SetInfoText (string text) {
			infoLabel.text = text;
		}

		public void Reset (bool clearInputFields) {
			ToggleInputs(true);
			SetInfoText("Login");

			if (clearInputFields) {
				inputUsername.value = "";
				inputPassword.value = "";
			}
		}
	}

}