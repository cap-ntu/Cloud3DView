using UnityEngine;

/// <summary>
/// Generic chat window functionality.
/// </summary>

public class UIChat : MonoBehaviour
{
	static UIChat mInst;

	/// <summary>
	/// Font used by the labels created by the chat window.
	/// </summary>

	public UIFont font;

	/// <summary>
	/// Input field used for chat input.
	/// </summary>

	public UIInput input;

	/// <summary>
	/// Chat window's background sprite, if one is desired. it will be resized dynamically.
	/// </summary>

	public UISprite background;

	/// <summary>
	/// Padding height used by the background. This value is added to the height of the printed text.
	/// </summary>

	public int backgroundPadding = 4;

	/// <summary>
	/// Root object for chat window's history. This allows you to position the chat window's text.
	/// </summary>

	public GameObject history;

	/// <summary>
	/// Maximum number of lines kept in the chat window before they start getting removed.
	/// </summary>

	public int maxLines = 10;

	/// <summary>
	/// Maximum width of each line.
	/// </summary>

	public int lineWidth = 0;

	/// <summary>
	/// Seconds that must elapse before a chat label starts to fade out.
	/// </summary>

	public float fadeOutStart = 10f;

	/// <summary>
	/// How long it takes for a chat label to fade out in seconds.
	/// </summary>

	public float fadeOutDuration = 5f;

	/// <summary>
	/// Whether messages will fade out over time.
	/// </summary>

	public bool allowChatFading = true;

	/// <summary>
	/// Whether the activate the chat input when Return key gets pressed.
	/// </summary>

	public bool activateOnReturnKey = true;

	class ChatEntry
	{
		public UILabel label;
		public Color color;
		public float time;
		public int lines = 0;
		public float alpha = 0f;
		public bool isExpired = false;
		public bool shouldBeDestroyed = false;
		public bool fadedIn = false;
	}

	BetterList<ChatEntry> mChatEntries = new BetterList<ChatEntry>();
	int mBackgroundHeight = -1;
	bool mIgnoreNextEnter = false;

	void Awake ()
	{
		mInst = this;

		if (input != null)
		{
			EventDelegate.Set(input.onSubmit, OnSubmitInternal);
			input.defaultText = Localization.Get("Chat");
			UIEventListener.Get(input.gameObject).onSelect = OnSelectInput;
		}
	}

	void OnDestroy () { mInst = null; }

	/// <summary>
	/// Make sure the background is of proper size.
	/// </summary>

	void Start () { ResizeBackground(0, true); }

	/// <summary>
	/// Chat shouldn't be fading while the input field has focus.
	/// </summary>

	void OnSelectInput (GameObject go, bool isSelected) { allowChatFading = !isSelected; }

	/// <summary>
	/// Send a chat message to everyone.
	/// </summary>

	void OnSubmitInternal ()
	{
		string text = UIInput.current.value;

		if (!string.IsNullOrEmpty(text))
		{
			text = NGUIText.StripSymbols(text);
			input.value = "";
			OnSubmit(text);
		}
		mIgnoreNextEnter = true;
	}

	/// <summary>
	/// Custom submit logic for what happens on chat input submission.
	/// </summary>

	protected virtual void OnSubmit (string text) { }

	/// <summary>
	/// Add a new chat entry.
	/// </summary>

	void InternalAdd (string text, Color color)
	{
		ChatEntry ent = new ChatEntry();
		ent.time = RealTime.time;
		ent.color = color;
		mChatEntries.Add(ent);
		
		GameObject go = NGUITools.AddChild(history != null ? history : gameObject);
		ent.label = go.AddComponent<UILabel>();
		ent.label.pivot = UIWidget.Pivot.BottomLeft;
		ent.label.cachedTransform.localScale = new Vector3(1f, 0.001f, 1f);
		ent.label.width = lineWidth;
		ent.label.bitmapFont = font;
		ent.label.color = ent.label.bitmapFont.premultipliedAlphaShader ? new Color(0f, 0f, 0f, 0f) : new Color(color.r, color.g, color.b, 0f);
		ent.label.text = text;
		ent.label.MakePixelPerfect();
		ent.lines = ent.label.processedText.Split('\n').Length;

		for (int i = mChatEntries.size, lineOffset = 0; i > 0; )
		{
			ChatEntry e = mChatEntries[--i];

			if (i + 1 == mChatEntries.size)
			{
				// It's the first entry. It doesn't need to be re-positioned.
				lineOffset += e.lines;
			}
			else
			{
				// This is not a first entry. It should be tweened into its proper place.
				int pixelOffset = lineOffset * font.defaultSize;
                
				if (lineOffset + e.lines > maxLines)
				{
					e.isExpired = true;
					e.shouldBeDestroyed = true;

					if (e.alpha == 0f)
					{
						mChatEntries.RemoveAt(i);
						Destroy(e.label.gameObject);
						continue;
					}
				}
				lineOffset += e.lines;
				TweenPosition.Begin(e.label.gameObject, 0.2f, new Vector3(0f, pixelOffset, 0f));
			}
		}
	}

	/// <summary>
	/// Update the "alpha" of each line and update the background size.
	/// </summary>

	protected void Update ()
	{
		if (activateOnReturnKey && Input.GetKeyUp(KeyCode.Return))
		{
			if (!mIgnoreNextEnter) input.isSelected = true;
			mIgnoreNextEnter = false;
		}

		int height = 0;

		for (int i = 0; i < mChatEntries.size; )
		{
			ChatEntry e = mChatEntries[i];
			float alpha;

			if (e.isExpired)
			{
				// Quickly fade out expired chat entries
				alpha = Mathf.Clamp01(e.alpha - RealTime.deltaTime);
			}
			else if (!allowChatFading || RealTime.time - e.time < fadeOutStart)
			{
				// Quickly fade in new entries
				alpha = Mathf.Clamp01(e.alpha + RealTime.deltaTime * 5f);
			}
			else if (RealTime.time - (e.time + fadeOutStart) < fadeOutDuration)
			{
				// Slowly fade out entries that have been visible for a while
				alpha = Mathf.Clamp01(e.alpha - RealTime.deltaTime / fadeOutDuration);
			}
			else
			{
				// Quickly fade out chat entries that should have faded by now,
				// but likely didn't due to the input being selected.
				alpha = Mathf.Clamp01(e.alpha - RealTime.deltaTime);
			}

			if (e.alpha != alpha)
			{
				e.alpha = alpha;

				if (!e.fadedIn && !e.isExpired)
				{
					// This label has not yet faded in, we want to scale it in, 
					// as it looks better and goes well with the scaled background.
					float labelHeight = Mathf.Lerp(0.001f, 1f, e.alpha);
					e.label.cachedTransform.localScale = new Vector3(1f, labelHeight, 1f);
				}

				// Fade in or fade out the label
				if (e.label.bitmapFont.premultipliedAlphaShader)
				{
					e.label.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), e.color, e.alpha);
				}
				else
				{
					e.label.alpha = e.alpha;
				}

				if (alpha == 1f)
				{
					// The chat entry has faded in fully
					e.fadedIn = true;
				}
				else if (alpha == 0f && e.shouldBeDestroyed)
				{
					// This chat entry has expired and should be removed
					mChatEntries.RemoveAt(i);
					Destroy(e.label.gameObject);
					continue;
				}
			}

			// If the line is visible, it should be counted
			if (e.alpha > 0.01f) height += e.lines * font.defaultSize;
			++i;
		}

		// Resize the background if its height has changed
		if (background != null && mBackgroundHeight != height)
			ResizeBackground(height, !allowChatFading);
	}

	/// <summary>
	/// Resize the background to fit the specified height in pixels.
	/// </summary>

	void ResizeBackground (int height, bool instant)
	{
		mBackgroundHeight = height;

		if (height == 0)
		{
			if (instant)
			{
				background.height = 2;
				background.enabled = false;
			}
			else
			{
				UITweener tween = TweenHeight.Begin(background, 0.2f, 2);
				EventDelegate.Add(tween.onFinished, DisableBackground, true);
			}
		}
		else
		{
			background.enabled = true;

			if (instant)
			{
				UITweener tween = background.GetComponent<TweenScale>();
				if (tween != null) tween.enabled = false;
				background.height = height + backgroundPadding;
			}
			else TweenHeight.Begin(background, 0.2f, height + backgroundPadding);
		}
	}

	/// <summary>
	/// When the background resizing tween finishes, disable the background.
	/// </summary>

	void DisableBackground () { background.enabled = false; }

	/// <summary>
	/// Add a new chat entry.
	/// </summary>

	static public void Add (string text) { Add(text, new Color(0.7f, 0.7f, 0.7f, 1f)); }

	/// <summary>
	/// Add a new chat entry.
	/// </summary>

	static public void Add (string text, Color color) { if (mInst != null) mInst.InternalAdd(text, color); }
}
