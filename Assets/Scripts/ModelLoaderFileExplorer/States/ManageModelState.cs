using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using FiniteStateMachine.Unity;


namespace ModelLoaderFileExplorer {

	public class ManageModelState : MonoBehaviour {

		public UIGrid grid;
		public ModelDataWidget prefab;
		public UIPanel toWindowWhenClicked;
		public UILabel infoLabel;
		public UploadModelControllerComponent controller;
		public Transform modelContainer;
		
		private List<ModelData> _modelDataList;
		
		private State _state;
		
		
		void OnEnable () {
			GetModelDataListFromCloud ();
			_state = State.Waiting;
		}
		
		void Update () {
			if (_modelDataList != null) {
				CreateModelDataWidgetList();
				_modelDataList = null;
			}
			
			switch (_state) {
			case State.Waiting:
				infoLabel.text = "Loading...";
				break;
			case State.Failed:
				infoLabel.text = "Loading Failed";
				break;
			default:
				infoLabel.text = "";
				break;
			}
		}
		
		void OnDisable () {
			foreach (Transform trans in grid.transform) {
				Destroy(trans.gameObject);
			}
		}
		
		private void GetModelDataList () {
			_modelDataList = new List<ModelData>();
			
			List<string> dataStringList = PersistanceManager.GetLocalModelData();
			foreach (string dataString in dataStringList) {
				ModelData modelData = ModelData.Deserialize(dataString);
				_modelDataList.Add(modelData);
				Debug.Log(modelData.name);
			}
		}
		
		private void GetModelDataListFromCloud () {
			_state = State.Waiting;
			
			CloudData.GetUserModelDataListFromCloud(modelDatas => {
				Debug.Log("Get Model Data List form Cloud Success");
				_modelDataList = modelDatas;
				_state = State.Success;
			}, 
			message => {
				Debug.Log("Get Model Data List form Cloud Failed");
				Debug.Log("Exception: " + message);
				_state = State.Failed;
			});
		}
		
		private void CreateModelDataWidgetList () {
			
			foreach (ModelData modelData in _modelDataList) {
				ModelDataWidget widget = (ModelDataWidget) GameObject.Instantiate(prefab);
				widget.modelData = modelData;
				widget.toWindow = toWindowWhenClicked;
				widget.button.onClick.Add(new EventDelegate(this, "CreateModel"));
				
				grid.AddChild(widget.transform);
				widget.transform.localScale = new Vector3(1, 1, 1);
			}
			
			grid.Reposition();
		}
		
		private void CreateModel () {
			var handler = toWindowWhenClicked.GetComponent<ViewModelState>();
			var widget = UIButton.current.GetComponent<ModelDataWidget>();
			widget.modelData.modelPath = PersistanceManager.GetLocalServerModelPath(widget.modelData.name);
			handler.modelData = widget.modelData;
			
			StartCoroutine(CloudFile.DownloadFile(widget.modelData.modelFile, widget.modelData.modelUrl, () => {
				var parts = widget.modelData.modelFile.Split('.');
				var path = Path.Combine(Utilities.GetPersistancePath(), "model." + parts[parts.Length-1]);
				var task = (RMITask) Instantiate(controller.importTask);
                Utilities.LoadModelFileRMI(path, task, CreateModelCallback);
			}));
		}
		
		private void CreateModelCallback (RMI.RMITask inTask) {
			GameObject modelGo = inTask.ImportedObject;
			Utilities.SetLayerRecursively(modelGo, LayerMask.NameToLayer("UI 3D"));
			modelGo.transform.parent = modelContainer;
			modelGo.transform.localPosition = controller.modelPosition;
			modelGo.name = "View State Model";
			Utilities.NormalizeMeshSize(modelGo);
			
			var windowButton = GetComponent<UIWindowButton>();
			windowButton.action = UIWindowButton.Action.Show;
			windowButton.WindowAction();
			windowButton.action = UIWindowButton.Action.DoNothing;
		}
		
		private void CreateModelForTest () {
			GameObject go = new GameObject("View State Model");
			
			var handler = toWindowWhenClicked.GetComponent<ViewModelState>();
			var widget = UIButton.current.GetComponent<ModelDataWidget>();
			widget.modelData.modelPath = PersistanceManager.GetLocalServerModelPath(widget.modelData.name);
			handler.modelData = widget.modelData;
		}
	}

}