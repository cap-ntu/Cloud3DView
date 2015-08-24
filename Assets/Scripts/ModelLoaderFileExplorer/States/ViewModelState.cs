using UnityEngine;
using System.Collections;
using FiniteStateMachine.Unity;

namespace ModelLoaderFileExplorer {

	[RequireComponent (typeof(ViewModelInputsHelper))]
	public class ViewModelState : MonoBehaviour {
		public UIButton submitButton;
		public Camera modelCamera;
		public GameObject dragAreaGo;
		public float zoomSpeed;
		public float rotateSpeed;
		public float moveSpeed;
		public string modelName;
		public float viewSizeMax;
		public float viewSizeMin;

		private ViewModelInputsHelper _helper;

		private float _originFOV;
		private Vector2 _previousMousePosition;
		private ModelData _modelData;
		private Transform _modelTrans;
		private bool _preserveModelData;

		public ModelData modelData {
			get {return _modelData;}
			set {
				_preserveModelData = true;
				_modelData = value;
				_helper = GetComponent<ViewModelInputsHelper>();
				_helper.UpdateInputsData(value);
			}
		}

		
		void OnEnable () {
			_originFOV = modelCamera.fieldOfView;

			GameObject modelGo = (GameObject) GameObject.Find(modelName);
			if (modelGo == null) {
				Debug.LogError("Cannot find GameObject named after '" + modelName + "'");
			}
			else {
				_modelTrans = modelGo.transform;
			}

			if (_modelTrans.renderer != null) {
				float width = _modelTrans.renderer.bounds.size.x;
				float height = _modelTrans.renderer.bounds.size.y;

				Debug.Log(width);
				while (width > viewSizeMax || height > viewSizeMax) {
					Vector3 scale = _modelTrans.localScale;
					scale *= 0.5f;
					_modelTrans.localScale = scale;
					width *= 0.5f;
					height *= 0.5f;
				}

				while (width < viewSizeMin || height < viewSizeMin) {
					Vector3 scale = _modelTrans.localScale;
					scale *= 2;
					_modelTrans.localScale = scale;
					width *= 2f;
					height *= 2f;
				}
			}

			if (!_preserveModelData) {
				_modelData = new ModelData();
				_helper = GetComponent<ViewModelInputsHelper>();
				_helper.UpdateInputsData(_modelData);
			}
		}
		
		void Update () {
			if (Check()) {
//				UpdateFieldOfView();
				UpdateRotation();
//				UpdatePosition();
			}
		}
		
		void OnDisable () {
			if (modelCamera != null) {
				modelCamera.fieldOfView = _originFOV;
			}

			submitButton.onClick.Clear();

			_preserveModelData = false;
//			if (_modelTrans != null) {
//				GameObject.Destroy(_modelTrans.gameObject);
//			}
		}
		
		private void UpdateFieldOfView () {
			modelCamera.fieldOfView += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		}
		
		private void UpdateRotation () {
			if (Input.GetMouseButtonDown(0)) {
				_previousMousePosition = GetMousePoisition();
			}
			else if (Input.GetMouseButton(0)) {
				Vector2 mousePosition = GetMousePoisition();
				
				Vector2 delta = mousePosition - _previousMousePosition;
				Vector3 angles = _modelTrans.eulerAngles;
				
				angles.x += delta.y * rotateSpeed;
				angles.y += delta.x * rotateSpeed;
				
				_modelTrans.eulerAngles = angles;
				
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
				Vector3 position = _modelTrans.position;
				
				position.x += delta.x * moveSpeed;
				position.y += delta.y * moveSpeed;
				
				_modelTrans.position = position;
				
				_previousMousePosition = mousePosition;
			}
		}
		
		private Vector2 GetMousePoisition () {
			Vector3 mousePositionInScreenUnit = Input.mousePosition;
			mousePositionInScreenUnit.z = Mathf.Abs(modelCamera.transform.position.z);
			return modelCamera.ScreenToWorldPoint(mousePositionInScreenUnit);
		}

		private bool Check () {
			return UICamera.hoveredObject == dragAreaGo;
		}

		public void OnSubmitButtonClicked () {
			_helper.SubmitFields();

			if (_modelData.username == null || _modelData.username.Equals("")) {	// New server, upload model
				_modelData.modelPath = UploadModelController.previousLoadedModelPath;
			}

			//_modelData.Serialize();
			_modelData.MakeCloud();
		}

		public void OnAddToMineButtonClicked () {
			_modelData.AddToUser();
		}

		public void OnToggle () {
			if (UIToggle.current.name.Contains("Input - Is Private")) {
				_modelData.isPrivate = UIToggle.current.value;
			}
		}

		public void OnInputFieldSubmit () {
			if (!enabled) return;

			UIInput input = UIInput.current;

			if (input.value == null || input.value.Equals("") || _modelData == null) return;

			if (input.name.Contains("Input - Model")) {
				_modelData.name = input.value;
			}
			else if (input.name.Contains("Input - Unit Size")) {
				_modelData.unitSize = float.Parse(input.value);
			}
			else if (input.name.Contains("Input - CPU Core")) {
				_modelData.cpuCoresCount = int.Parse(input.value);
			}
			else if (input.name.Contains("Input - CPU Hz")) {
				_modelData.cpuHz = float.Parse(input.value);
			}
			else if (input.name.Contains("Input - HDD Bays")) {
				_modelData.hddBaysCount = int.Parse(input.value);
			}
			else if (input.name.Contains("Input - Memory Speed")) {
				_modelData.memorySpeed = int.Parse(input.value);
			}
			else if (input.name.Contains("Input - Memory Capacity Count")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.memoryCapacity.Count) {
					_modelData.memoryCapacity.Add(new PairData(0, 0));
				}

				PairData pairData = _modelData.memoryCapacity[i];
				pairData.count = int.Parse(input.value);
				_modelData.memoryCapacity[i] = pairData;
			}
			else if (input.name.Contains("Input - Memory Capacity Data")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.memoryCapacity.Count) {
					_modelData.memoryCapacity.Add(new PairData(0, 0));
				}
				
				PairData pairData = _modelData.memoryCapacity[i];
				pairData.data = int.Parse(input.value);
				_modelData.memoryCapacity[i] = pairData;
			}
			else if (input.name.Contains("Input - RAID")) {
				_modelData.raid = input.value;
			}
			else if (input.name.Contains("Input - HDD Count")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.hdd.Count) {
					_modelData.hdd.Add(new PairData(0, 0));
				}
				
				PairData pairData = _modelData.hdd[i];
				pairData.count = int.Parse(input.value);
				_modelData.hdd[i] = pairData;
			}
			else if (input.name.Contains("Input - HDD Data")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.hdd.Count) {
					_modelData.hdd.Add(new PairData(0, 0));
				}
				
				PairData pairData = _modelData.hdd[i];
				pairData.data = int.Parse(input.value);
				_modelData.hdd[i] = pairData;
			}
			else if (input.name.Contains("Input - Network Count")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.network.Count) {
					_modelData.network.Add(new PairData(0, 0));
				}
				
				PairData pairData = _modelData.network[i];
				pairData.count = int.Parse(input.value);
				_modelData.network[i] = pairData;
			}
			else if (input.name.Contains("Input - Network Data")) {
				string parentName = input.transform.parent.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (parentName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.network.Count) {
					_modelData.network.Add(new PairData(0, 0));
				}
				
				PairData pairData = _modelData.network[i];
				pairData.data = int.Parse(input.value);
				_modelData.network[i] = pairData;
			}
			else if (input.name.Contains("Input - GPU")) {
				string inputName = input.name;
				bool is0 = true;
				int i=0;
				for (; i<100; i++) {
					if (inputName.Contains(i.ToString())) {
						is0 = false;
						break;
					}
				}
				if (is0) {
					i = 0;
				}
				if (i >= _modelData.gpu.Count) {
					_modelData.gpu.Add("");
				}
				
				_modelData.gpu[i] = input.value;
			}

			
			Debug.Log(_modelData.ToString());
		}
	}

}