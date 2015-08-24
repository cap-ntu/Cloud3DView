using UnityEngine;
using System.Collections;
using FiniteStateMachine;

namespace ModelLoaderFileExplorer {

	public class ModelImportManager : MonoBehaviour {

		public Vector3 modelPosition;
		public float modelMoveSpeed = 1;
		public float modelRotateSpeed = 1;
		public float modelZoomSpeed = 1;
		public GameObject importModelButtonGo;
		public Camera viewingCamera;
		public GameObject inputFieldsContainerGo;
		public UIProgressBar progressBar;

		[HideInInspector]
		public GameObject modelGo;

		private FSMMachine _fsm;

		private static ModelImportManager _instance;


		public static ModelImportManager instance {
			get {
				if (_instance == null) {
					Debug.LogError("Model Import Manager is not in the scene yet.");
				}
				return _instance;
			}
		}

		
		void Awake () {
			_instance = this;
		}

		void Start () {
			_fsm = new FSMMachine();

			IdleState idle = new IdleState();
			SelectState selecting = new SelectState();
			ViewState viewing = new ViewState();
			UploadState uploading = new UploadState();

			_fsm.AddState(idle);
			_fsm.AddState(selecting);
			_fsm.AddState(viewing);
			_fsm.AddState(uploading);

			_fsm.initialCurrentState = idle;
		}
		
		void Update () {
			_fsm.Update();
		}

		public void ChangeState (string toState) {
			_fsm.ChangeState(toState);
		}

		public void ChangeStateAfterSeconds (string toState, float seconds) {
			StartCoroutine(ChangeStateAfterSecondsCoroutine(toState, seconds));
		}

		private IEnumerator ChangeStateAfterSecondsCoroutine (string toState, float seconds) {
			yield return new WaitForSeconds(seconds);

			_fsm.ChangeState(toState);
		}
	}

}
