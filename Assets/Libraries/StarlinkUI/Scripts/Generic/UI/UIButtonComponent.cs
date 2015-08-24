using UnityEngine;

/// <summary>
/// Enable or disable the target component on click.
/// </summary>

public class UIButtonComponent : MonoBehaviour
{
	public enum Action
	{
		Enable,
		Disable,
		Toggle,
	}

	public MonoBehaviour target;
	public Action action = Action.Disable;

	void Awake () { Debug.Log("Needed!"); }

	void OnClick ()
	{
		if (target != null)
		{
			if (action == Action.Disable)
			{
				target.enabled = false;
			}
			else if (action == Action.Enable)
			{
				target.enabled = true;
			}
			else
			{
				target.enabled = !target.enabled;
			}
		}
	}
}
