using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NGUIHelper {

	public class InputFieldToggler : MonoBehaviour {
		public int index;
		public InputFieldToggler parent;
		public UIWidget prefab;
		public float step;

		public List<UIWidget> widgets;



		public void AddField () {
			UIWidget widget = (UIWidget) GameObject.Instantiate(prefab);

			widget.transform.parent = prefab.transform.parent;

			Vector3 localPosition = prefab.transform.localPosition;
			localPosition.y += prefab.transform.localPosition.y + step * widgets.Count;
			widget.transform.localPosition = localPosition;

			widget.transform.rotation = prefab.transform.rotation;
			widget.transform.localScale = prefab.transform.localScale;
			widget.name = prefab.name + (widgets.Count);

			widget.gameObject.SetActive(true);

			UIInput[] inputs = widget.GetComponentsInChildren<UIInput>(true);
			foreach (UIInput input in inputs) {
				input.label.text = "";
				input.value = "";
			}

			widgets.Add(widget);

			if (parent != null) {
				parent.AdjustFieldSpace(index + 1, true);
			}
		}

		public void DeleteLastField () {
			var widget = widgets[widgets.Count - 1];
			widgets.RemoveAt(widgets.Count - 1);
			Destroy(widget.gameObject);
			
			if (parent != null) {
				parent.AdjustFieldSpace(index + 1, false);
			}
		}

		private void AdjustFieldSpace (int widgetIndex, bool add) {
			for (int i=widgetIndex; i<widgets.Count; i++) {
				Vector3 localPosition = widgets[i].transform.localPosition;
				if (add) {
					localPosition.y += step;
				}
				else {
					localPosition.y -= step;
				}
				widgets[i].transform.localPosition = localPosition;
			}
		}
	}

}