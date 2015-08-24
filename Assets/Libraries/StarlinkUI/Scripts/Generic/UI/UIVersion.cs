using UnityEngine;

/// <summary>
/// This very simple script makes it trivial to keep a local version number easily accessible from within your code.
/// </summary>

[RequireComponent(typeof(UILabel))]
public class UIVersion : MonoBehaviour
{
	/// <summary>
	/// Game's version number. You should update this value number every time you do a build.
	/// </summary>

	static public int buildID = 100;

	void OnEnable ()
	{
		UILabel lbl = GetComponent<UILabel>();
		lbl.text = string.Format("{0} {1}", Localization.Get("Build"), buildID);
	}
}
