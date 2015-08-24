using UnityEngine;

/// <summary>
/// This script is used to create a status bar style widget that fades in, shows the message, waits a bit then fades out.
/// Basic usage: UIStatus.Show(text)
/// </summary>

public class UIStatus : MonoBehaviour
{
	static UIStatus mInst;

	/// <summary>
	/// Label for the status bar's text.
	/// </summary>

	public UILabel label;

	/// <summary>
	/// Background sprite for the status bar.
	/// </summary>

	public UISprite background;

	/// <summary>
	/// Animation curve applied to the fading-in process.
	/// </summary>

	public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	/// <summary>
	/// Time it takes for a new message to pop in.
	/// </summary>

	public float fadeInTime = 0.25f;

	/// <summary>
	/// How long a message will remain shown if there is no other message waiting.
	/// </summary>

	public float shownDuration = 4f;

	/// <summary>
	/// How long it takes for a message to fade out when another one is waiting.
	/// </summary>

	public float transitionTime = 0.25f;

	/// <summary>
	/// How long it takes for a message to fade out when there are no other messages waiting.
	/// </summary>

	public float fadeOutTime = 4f;

	/// <summary>
	/// Padding for the border.
	/// </summary>

	public Vector2 borderPadding = Vector2.zero;

	float mShownTime = 0f;
	float mAlpha = 0f;
	bool mFadingIn = false;
	bool mForceFadeOut = false;

	struct Entry
	{
		public string text;
		public Color color;
		public bool persistent;
	}

	static BetterList<Entry> mEntries = new BetterList<Entry>();
	Entry mCurrent;
	Transform mTrans;
	Color mInvisible = new Color(0f, 0f, 0f, 0f);
	Color mBackColor = Color.black;

	void Awake () { mInst = this; mTrans = transform; mBackColor = background.color; }
	void Start () { mEntries.Clear(); label.enabled = false; background.enabled = false; }
	void OnDestroy () { if (mInst == this) mInst = null; }

	/// <summary>
	/// Force-set the visible text value.
	/// </summary>

	void UpdateText ()
	{
		// Update the label
		label.text = mCurrent.text;
		label.color = mCurrent.color;
		label.enabled = true;

		// Resize the background
		Vector2 size = label.localSize;
		Vector4 border = background.border;
		size.x += border.x + border.z + borderPadding.x;
		size.y += border.y + border.w + borderPadding.y;

		background.width = Mathf.RoundToInt(size.x);
		background.height = Mathf.RoundToInt(size.y);
		background.enabled = true;
	}

	/// <summary>
	/// Animate the status bar.
	/// </summary>

	void Update ()
	{
		if (mAlpha == 0f)
		{
			if (mEntries.size > 0)
			{
				// We have something to show -- let's start fading it in
				mCurrent = mEntries[0];
				mEntries.RemoveAt(0);
				mFadingIn = true;
				mForceFadeOut = false;
				UpdateText();
			}
			else if (!mFadingIn) return;
		}

		float time = Time.realtimeSinceStartup;

		// If the current entry is persistent and we have nothing more to show, keep updating the time
		if (mCurrent.persistent && mEntries.size == 0) mShownTime = time;

		// Calculate this entry's expiration time
		if (mEntries.size > 0) mForceFadeOut = true;
		float expireTime = mForceFadeOut ? mShownTime : mShownTime + shownDuration;

		if (mFadingIn)
		{
			mAlpha += RealTime.deltaTime / fadeInTime;

			if (mAlpha > 1f)
			{
				// We're done fading in
				mTrans.localScale = Vector3.one;
				background.color = mBackColor;
				label.color = mCurrent.color;
				mShownTime = time;
				mFadingIn = false;
			}
			else
			{
				// Still fading in
				float amount = curve.Evaluate(mAlpha);
				mTrans.localScale = new Vector3(0.2f + 0.8f * amount, 1f, 1f);
				background.color = Color.Lerp(mInvisible, mBackColor, amount);
				label.color = Color.Lerp(mInvisible, mCurrent.color, amount * amount);
			}
		}
		else if (expireTime < time)
		{
			mAlpha -= RealTime.deltaTime / (mForceFadeOut ? transitionTime : fadeOutTime);

			if (mAlpha < 0.01f)
			{
				// We're done fading out
				label.enabled = false;
				background.enabled = false;
				mAlpha = 0f;
			}
			else
			{
				// Still fading out
				background.color = Color.Lerp(mInvisible, mBackColor, mAlpha);
				label.color = Color.Lerp(mInvisible, mCurrent.color, mAlpha * mAlpha);
			}
		}
	}

	/// <summary>
	/// Show a new entry on the status bar.
	/// </summary>

	static public void Show (string text) { Show(text, new Color(0.7f, 0.7f, 0.7f), false, false); }

	/// <summary>
	/// Show a new entry on the status bar.
	/// </summary>

	static public void Show (string text, Color c) { Show(text, c, false, false); }

	/// <summary>
	/// Show a new entry on the status bar.
	/// </summary>

	static public void Show (string text, Color c, bool persistent, bool instant)
	{
		if (mInst != null && instant && (mInst.mFadingIn || mInst.mCurrent.color.a == 1f))
		{
			mEntries.Clear();
			mInst.mCurrent.text = text;
			mInst.mCurrent.color = c;
			mInst.UpdateText();
		}
		else
		{
			Entry ent = new Entry();
			ent.text = text;
			ent.color = c;
			ent.persistent = persistent;
			mEntries.Add(ent);
			if (mInst != null) mInst.mForceFadeOut = false;
		}
	}

	/// <summary>
	/// Hide currently shown text.
	/// </summary>

	static public void Hide ()
	{
		mEntries.Clear();

		if (mInst != null)
		{
			mInst.mCurrent.persistent = false;
			mInst.mFadingIn = false;
			mInst.mForceFadeOut = true;
		}
	}
}
