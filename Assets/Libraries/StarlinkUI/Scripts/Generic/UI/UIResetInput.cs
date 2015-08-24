using UnityEngine;

/// <summary>
/// Very trivial script that resets the target input's text when the object with this script gets clicked on.
/// </summary>

public class UIResetInput : MonoBehaviour
{
	public UIInput input;

	void OnClick ()
	{
		input.value = "";
	}
}
