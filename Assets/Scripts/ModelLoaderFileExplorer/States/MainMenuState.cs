using UnityEngine;
using System.Collections;
using Cloud;

namespace ModelLoaderFileExplorer {

	public class MainMenuState : MonoBehaviour {


		void Start () {
		
		}

		void Update () {
		
		}

		public void Logout () {
			CloudUser.Logout();
		}
	}

}