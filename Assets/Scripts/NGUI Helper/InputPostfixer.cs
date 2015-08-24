using UnityEngine;
using System.Collections;

namespace NGUIHelper {

	public class InputPostfixer : MonoBehaviour {

		public string postfix;
		public UILabel postfixLabel;


		void Start () {
			
		}
		
		public void OnChange (UIInput input) {
			string text = "";
			for (int i=0; i<=input.value.Length; i++) {
				text += "  ";
			}
			text += postfix;
			postfixLabel.text = text;
		}
	}

}