using UnityEngine;
using System.Collections;
using FiniteStateMachine.Unity;
using Cloud;

namespace ModelLoaderFileExplorer {

	[RequireComponent (typeof(LoginStateHelper))]
	public class LoginState : MonoBehaviour {

		public UIWindowButton button;
		public UILabel infoLabel;

		private LoginStateHelper _helper;

		private string _username;
		private string _password;
		private State _state;

		void Start () {
			_helper = GetComponent<LoginStateHelper>();
		}

		void OnEnable () {
			_state = State.Idle;
		}

		void OnDisable () {
			_helper.Reset(true);
		}

		void Update () {
			switch (_state) {
			case State.Success:
				button.collider.enabled = true;
				infoLabel.text = "";
				button.WindowAction();
				break;
			case State.Waiting:
				button.collider.enabled = false;
				infoLabel.text = "Please wait...";
				break;
			case State.Failed:
				button.collider.enabled = true;
				infoLabel.text = "Login failed.";
				_helper.Reset(false);
				break;
			}
		}



		public void SetUsername () {
			_username = UIInput.current.value;
		}

		public void SetPassword () {
			_password = UIInput.current.value;
		}

		public void Login () {
			_helper.ToggleInputs(false);
			_helper.SetInfoText("Logging in...");

			IUIWindowButton iButton = (IUIWindowButton) button;

			_state = State.Waiting;

//			_username = "Yifeng";
//			_password = "12345678";

			CloudUser.Login(_username, _password, () => {
				_state = State.Success;
			}, s => {
				_state = State.Failed;
				Debug.Log(s);
			});
		}
	}

}