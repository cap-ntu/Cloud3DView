using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModelLoaderFileExplorer {

	public class ToggleGroup : MonoBehaviour {

		public List<UIToggle> toggles;
		public int defaultToggleIndex = 0;

		private UIToggle _previousActiveToggle;

		/// <summary>
		/// Gets or sets the value.
		/// Value is the current result of the toggle group.
		/// </summary>
		public string value {get; set;}


		void Start () {
			UIToggle defaultToggle = toggles[defaultToggleIndex];
			defaultToggle.value = true;
			_previousActiveToggle = defaultToggle;


			foreach (UIToggle toggle in toggles) {
				toggle.onChange.Add(new EventDelegate(OnToggleChanged));
			}
		}
		
		private void OnToggleChanged () {
			if (_previousActiveToggle != null) {
				_previousActiveToggle.value = false;
			}

			_previousActiveToggle = UIToggle.current;
		}
	}

}
