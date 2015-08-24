using UnityEngine;

/// <summary>
/// This class facilitates an easy way of switching between different windows.
/// Use UIWindow.Show(panel) to show a window and UIWindow.GoBack() to go back to the previous one.
/// </summary>

public class UIWindow : MonoBehaviour
{
	static UIWindow mInst;
	static BetterList<UIPanel> mHistory = new BetterList<UIPanel>();
	static BetterList<UIPanel> mFading = new BetterList<UIPanel>();
	static UIPanel mActive;

	/// <summary>
	/// Currently visible window.
	/// </summary>

	static public UIPanel current { get { return mActive; } }

	/// <summary>
	/// Ensure we have an instance to work with.
	/// </summary>

	static void CreateInstance ()
	{
		if (mInst == null)
		{
			GameObject go = new GameObject("_UIWindow");
			mInst = go.AddComponent<UIWindow>();
            DontDestroyOnLoad(go);
		}
	}

	/// <summary>
	/// Ensure that the specified window has been added to the list.
	/// </summary>

	static public void Add (UIPanel window)
	{
		if (mActive == window) return;

		CreateInstance();

		if (mActive == null)
			mActive = window;
	}

	/// <summary>
	/// Show the specified window.
	/// </summary>

	static public void Show (UIPanel window)
	{
		if (mActive == window) return;

		CreateInstance();

		if (mActive != null)
		{
			mFading.Add(mActive);
			mHistory.Add(mActive);
		}

		if (mHistory.Remove(window))
		{
			mFading.Remove(window);
		}
		else if (window != null)
		{
            window.alpha = 0;
			//window. SetAlphaRecursive(0f, false);
		}

		mActive = window;
        if (mActive != null)
        {
            mActive.gameObject.SetActive(true);
        }
	}

	/// <summary>
	/// Hide the specified window, but only if the window is currently visible. If it's not, do nothing.
	/// </summary>

	static public void Hide (UIPanel window) { if (mActive == window) GoBack(); }

	/// <summary>
	/// Return to the previous window.
	/// </summary>

	static public void GoBack ()
	{
		CreateInstance();

		if (mHistory.size > 0)
		{
			if (mActive != null)
			{
				mFading.Add(mActive);
				mActive = null;
			}

			while (mActive == null)
			{
				mActive = mHistory.Pop();

				if (mActive != null)
				{
                    mActive.alpha = 0;
					//mActive.SetAlphaRecursive(0f, false);
					mActive.gameObject.SetActive(true);
					mFading.Remove(mActive);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Hide the current window and clear the history.
	/// </summary>

	static public void Close ()
	{
		if (mActive != null)
		{
			CreateInstance();
			mFading.Add(mActive);
			mHistory.Add(mActive);
			mActive = null;
		}
		mHistory.Clear();
	}

	/// <summary>
	/// Do the actual fading of panels.
	/// </summary>

	void Update ()
	{
		// Fade out the previous window
		for (int i = mFading.size; i > 0; )
		{
			UIPanel p = mFading[--i];

			if (p != null)
			{
                p.alpha = Mathf.Clamp01(p.alpha - RealTime.deltaTime * 6f);
				//p.SetAlphaRecursive(Mathf.Clamp01(p.alpha - RealTime.deltaTime * 6f), false);
				p.transform.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one, p.alpha);
				if (p.alpha > 0f) continue;
			}
			mFading.RemoveAt(i);
			p.gameObject.SetActive(false);
		}

		// Only fade in the new window after the previous has faded out
		if (mFading.size == 0 && mActive != null && mActive.alpha < 1f)
		{
            mActive.alpha = Mathf.Clamp01(mActive.alpha + RealTime.deltaTime * 6f);
			//mActive.SetAlphaRecursive(Mathf.Clamp01(mActive.alpha + RealTime.deltaTime * 6f), false);
			Transform t = mActive.transform;
			t.localScale = Vector3.Lerp(Vector3.one * 0.9f, Vector3.one, mActive.alpha);

			// 3D layer
			if (mActive.gameObject.layer == 10)
				t.localRotation = Quaternion.Euler(0f, -18f + mActive.alpha * 24f, 0f);
		}
	}
}
