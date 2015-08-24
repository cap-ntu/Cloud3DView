using UnityEngine;
using System.Collections;

public class CloudSettings : MonoBehaviour {

	public ServiceProvider serviceProvider;

	private static CloudSettings _instance;
	public static CloudSettings instance {get {return _instance;}}

	void Awake () {
		_instance = this;
	}
}

public enum ServiceProvider {
	Parse,
	LeanCloud,
}
