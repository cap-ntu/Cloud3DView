using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuntimeModelLoader.Ex;

public class TestModelLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject go = ModelLoaderEx.LoadModelGameObject("/Users/yifengwu/Desktop/test.obj");
		go.name = "Test Model";
	}

}
