using UnityEngine;
using System.Collections;
using FileExplorerNGUI.Ex;
using ModelLoaderFileExplorer;

public class TestFileExplorerNGUI : MonoBehaviour {

	public Vector3 modelPosition;

	// Use this for initialization
	void Start () {
		MLWindowController controller = new MLWindowController();
		FileExplorerNGUIEx.Open(controller);
		controller.modelPosition = modelPosition;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
