using UnityEngine;
using System.Collections;

/// <summary>
/// Simple script that is able to display current framerate.
/// </summary>

[RequireComponent(typeof(UILabel))]
public class UIFPS : MonoBehaviour
{
	int mFrames = 0;
	UILabel mLabel;

	void Start ()
	{
		mLabel = GetComponent<UILabel>();
		StartCoroutine(UpdateFPS());
	}

	IEnumerator UpdateFPS ()
	{
		for (; ; )
		{
			yield return new WaitForSeconds(1f);
			if (mLabel.enabled) mLabel.text = mFrames.ToString();
			mFrames = 0;
		}
	}

	void Update ()
	{
		if (Time.timeScale > 0.01f)
		{
			++mFrames;
		}
	}

	void OnClick ()
	{
		mLabel.enabled = !mLabel.enabled;
	}
}
