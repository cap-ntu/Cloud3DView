using UnityEngine;
using System.Collections;
using FiniteStateMachine;
using FileExplorerNGUI.Ex;
using ModelLoaderFileExplorer;

namespace ModelLoaderFileExplorer {

	public class StateBase : FSMState {
		protected ModelImportManager _manager;

		public StateBase () {
			_manager = ModelImportManager.instance;
		}
	}


	public class IdleState : StateBase {
		public IdleState () {
			this.name = Constants.IDLE_STATE;
		}

		public override void Enter () {
			base.Enter ();

			_manager.importModelButtonGo.SetActive(true);
		}

		public override void Exit () {
			base.Exit ();

			_manager.importModelButtonGo.SetActive(false);
		}
	}


	public class SelectState : StateBase {

		public SelectState () {
			this.name = Constants.SELECT_STATE;
		}

		public override void Enter () {
			base.Enter ();

			if (FileExplorerNGUIEx.CheckWindowOpened()) {
				FileExplorerNGUIEx.hidden = false;
			}
			else {
				MLWindowController controller = new MLWindowController();
				FileExplorerNGUIEx.Open(controller);
				controller.modelPosition = _manager.modelPosition;
			}
		}

		public override void Exit () {
			base.Exit();

			FileExplorerNGUIEx.hidden = true;
		}
	}


	public class ViewState : StateBase {
		private float _originFOV;
		private Vector2 _previousMousePosition;

		
		public ViewState () {
			this.name = Constants.VIEW_STATE;
		}
		
		public override void Enter () {
			base.Enter ();

			_originFOV = _manager.viewingCamera.fieldOfView;

			_manager.inputFieldsContainerGo.SetActive(true);
		}

		public override void Update () {
			base.Update ();

			UpdateFieldOfView();
			UpdateRotation();
			UpdatePosition();
		}

		public override void Exit () {
			base.Exit ();

			_manager.viewingCamera.fieldOfView = _originFOV;

			_manager.inputFieldsContainerGo.SetActive(false);

			GameObject.Destroy(_manager.modelGo);
		}

		private void UpdateFieldOfView () {
			_manager.viewingCamera.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * _manager.modelZoomSpeed;
		}

		private void UpdateRotation () {
			if (Input.GetMouseButtonDown(0)) {
				_previousMousePosition = GetMousePoisition();
			}
			else if (Input.GetMouseButton(0)) {
				Vector2 mousePosition = GetMousePoisition();

				Vector2 delta = mousePosition - _previousMousePosition;
				Vector3 angles = _manager.modelGo.transform.eulerAngles;

				angles.x += delta.y * _manager.modelRotateSpeed;
				angles.y += delta.x * _manager.modelRotateSpeed;

				_manager.modelGo.transform.eulerAngles = angles;

				_previousMousePosition = mousePosition;
			}
		}

		private void UpdatePosition () {
			if (Input.GetMouseButtonDown(1)) {
				_previousMousePosition = GetMousePoisition();
			}
			else if (Input.GetMouseButton(1)) {
				Vector2 mousePosition = GetMousePoisition();
				
				Vector2 delta = mousePosition - _previousMousePosition;
				Vector3 position = _manager.modelGo.transform.position;

				position.x += delta.x * _manager.modelMoveSpeed;
				position.y += delta.y * _manager.modelMoveSpeed;

				_manager.modelGo.transform.position = position;

				_previousMousePosition = mousePosition;
			}
		}

		private Vector2 GetMousePoisition () {
			Vector3 mousePositionInScreenUnit = Input.mousePosition;
			mousePositionInScreenUnit.z = Mathf.Abs(_manager.viewingCamera.transform.position.z);
			return _manager.viewingCamera.ScreenToWorldPoint(mousePositionInScreenUnit);
		}
	}


	public class UploadState : StateBase {

		public UploadState () {
			name = Constants.UPLOAD_STATE;
		}

		public override void Enter () {
			base.Enter ();

			_manager.progressBar.gameObject.SetActive(true);
			_manager.progressBar.value = 0;
		}

		public override void Update () {
			base.Update ();

			if (_manager.progressBar.value < 1) {
				float delta = Random.Range(0.003f, 0.015f);
				_manager.progressBar.value += delta;

				if (_manager.progressBar.value > 1) {
					_manager.progressBar.value = 1;
				}
			}
			else {
				_manager.ChangeStateAfterSeconds(Constants.IDLE_STATE, 1);
			}
		}


		public override void Exit () {
			base.Exit ();

			_manager.progressBar.gameObject.SetActive(false);
		}
	}

}
