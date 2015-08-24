using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NGUIHelper;

namespace ModelLoaderFileExplorer {

	public class ViewModelInputsHelper : MonoBehaviour {
		public UIInput nameField;
		public UIToggle isPrivateField;
		public UIInput unitSizeField;
		public UIInput cpuCoresField;
		public UIInput cpuHzField;
		public UIInput hddBaysField;
		public UIInput memorySpeedField;
		public InputFieldToggler memoryCapacityFields;
		public UIInput raidField;
		public InputFieldToggler hddFields;
		public InputFieldToggler networkFields;
		public InputFieldToggler gpuFields;

		private ViewModelState _viewModelState;


		void Start () {
			_viewModelState = GetComponent<ViewModelState>();
		}


		public void SubmitFields () {
			nameField.Submit();
			_viewModelState.modelData.isPrivate = isPrivateField.value;
			unitSizeField.Submit();
			cpuCoresField.Submit();
			cpuHzField.Submit();
			hddBaysField.Submit();
			memorySpeedField.Submit();

			SubmitInputFields(memoryCapacityFields);

			raidField.Submit();

			SubmitInputFields(hddFields);
			SubmitInputFields(networkFields);
			SubmitInputFields(gpuFields);
		}

		private void SubmitInputFields (InputFieldToggler toggler) {
			foreach (var container in toggler.widgets) {
				var fields = container.GetComponentsInChildren<UIInput>(true);
				foreach (var field in fields) {
					field.Submit();
				}
			}
		}

		public void UpdateInputsData (ModelData data) {
			nameField.value = data.name;
			isPrivateField.value = data.isPrivate;
			unitSizeField.value = data.unitSize.ToString();
			cpuCoresField.value = data.cpuCoresCount.ToString();
			cpuHzField.value = data.cpuHz.ToString();
			hddBaysField.value= data.hddBaysCount.ToString();
			memorySpeedField.value = data.memorySpeed.ToString();

			UpdateInputFieldsWithPairDatas(memoryCapacityFields, data.memoryCapacity);

			raidField.value = data.raid;

			UpdateInputFieldsWithPairDatas(hddFields, data.hdd);
			UpdateInputFieldsWithPairDatas(networkFields, data.network);
			UpdateInputFieldsWithString(gpuFields, data.gpu);
		}

		private void UpdateInputFieldsWithString (InputFieldToggler toggler, List<string> stringDatas) {
			for (int i=0; i<stringDatas.Count; i++) {
				UIWidget container;
				if (i > 0) {
					toggler.AddField();
					
					container = toggler.widgets[toggler.widgets.Count-1];
				}
				else {
					container = toggler.prefab;
				}
				
				var stringData = stringDatas[i];
				
				var fields = container.GetComponentsInChildren<UIInput>(true);
				foreach (var field in fields) {
					field.value = stringData;
				}
			}
		}

		private void UpdateInputFieldsWithPairDatas (InputFieldToggler toggler, List<PairData> pairDatas) {
			while ((toggler.widgets.Count > pairDatas.Count) && (toggler.widgets.Count != 1 && pairDatas.Count != 0)) {
				toggler.DeleteLastField();
			}

			for (int i=0; i<pairDatas.Count; i++) {
				UIWidget container;


				if (i >= toggler.widgets.Count) {
					toggler.AddField();

					container = toggler.widgets[toggler.widgets.Count-1];
				}
				else {
					container = toggler.widgets[i];
				}
				
				var dataPair = pairDatas[i];
				
				var fields = container.GetComponentsInChildren<UIInput>(true);
				foreach (var field in fields) {
					if (field.name.Contains("count") || field.name.Contains("Count")) {
						field.value = dataPair.count.ToString();
					}
					if (field.name.Contains("data") || field.name.Contains("Data")) {
						field.value = dataPair.data.ToString();
					}
				}
			}
		}
	}

}