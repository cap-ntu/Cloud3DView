using UnityEngine;

/// <summary>
/// Attach this component to a stand-alone game object and it will always follow the main camera.
/// </summary>

[RequireComponent(typeof(AudioListener))]
public class GameAudio : MonoBehaviour
{
	static GameAudio mInst;

	Transform mTrans;
	Transform mTarget;

	void Awake ()
	{
		if (mInst == null)
		{
			mInst = this;
			mTrans = transform;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	void LateUpdate ()
	{
		if (mTarget == null)
		{
			Camera cam = Camera.main;
			if (cam != null) mTarget = cam.transform;
		}

		if (mTarget != null)
		{
			if (mTrans.position != mTarget.position)
				mTrans.position = mTarget.position;
		}
	}
}