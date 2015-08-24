using UnityEngine;

public class UIMessageBox : MonoBehaviour
{
	public UIPanel panel;
	public UILabel title;
	public UILabel content;

	static UIMessageBox mInst;

	void Awake () { mInst = this; }
	void OnDestroy () { if (mInst == this) mInst = null; }

	static public void Show (string title, string content)
	{
		if (mInst != null)
		{
			mInst.title.text = title;
			mInst.content.text = content;
			UIWindow.Show(mInst.panel);
		}
	}
}
