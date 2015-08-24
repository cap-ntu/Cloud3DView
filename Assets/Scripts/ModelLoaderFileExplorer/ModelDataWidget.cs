using UnityEngine;
using System.Collections;

namespace ModelLoaderFileExplorer {

	public class ModelDataWidget : MonoBehaviour {

		public UILabel label;
		public UILabel labelInfo;
		public InfoType infoType;
		public UIWidget widget;
		public bool showUploaderDetail;

		public UIPanel toWindow {get; set;}

		public UIButton button {get; private set;}

		private ModelData _modelData;
		public ModelData modelData {
			get {return _modelData;}
			set {
				_modelData = value;
				label.text = _modelData.name;

				switch (infoType) {
				case InfoType.UserName:
					labelInfo.text = _modelData.username;
					break;
				case InfoType.IsPrivate:
					labelInfo.text = _modelData.isPrivate ? "Private" : "Public";
					break;
				}
			}
		}

		void Awake () {
			button = GetComponent<UIButton>();
		}

		void Start () {
			var button = GetComponent<UIWindowButton>();
		}

		public enum InfoType {
			UserName,
			IsPrivate
		}
	}

}