using UnityEngine;

/// <summary>
/// This class makes it possible to activate / deactivate a target object via a Unity animation.
/// TODO: Is this needed?
/// </summary>

public class SetActiveProperty : MonoBehaviour
{
	public GameObject target;
	public bool activeState = true;

	Collider mCol;

	void Awake () { mCol = collider; Debug.Log("Needed!"); }

	void Start () { Update(); }

	void Update()
	{
		if (target.activeSelf != activeState)
		{
			target.SetActive(activeState);
			if (mCol != null) mCol.enabled = activeState;
		}
	}
}
