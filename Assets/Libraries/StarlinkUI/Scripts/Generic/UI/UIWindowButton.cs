using UnityEngine;

public interface IUIWindowButton {
	void WindowAction();
}

public class UIWindowButton : MonoBehaviour, IUIWindowButton
{
	public enum Action
	{
		Show,
		Hide,
		GoBack,
        DoNothing,
	}

	public bool actOnClick = true;
	public UIPanel window;
	public Action action = Action.Hide;
	public bool eraseHistory = false;

	void Start ()
	{
		UIPanel panel = NGUITools.FindInParents<UIPanel>(gameObject);

		if (panel != null)
		{
			UIWindow.Add(panel);
		}
	}

	void OnClick ()
	{
        //if (requiresFullVersion && !PlayerProfile.fullAccess)
        //{
        //    UIUpgradeWindow.Show();
        //    return;
        //}
		if (actOnClick) {
			WindowAction();
		}
	}

    public void WindowAction()
    {
        switch (action)
        {
            case Action.Show:
                {
                    if (window != null)
                    {
                        if (eraseHistory) UIWindow.Close();
                        UIWindow.Show(window);
                    }
                }
                break;

            case Action.Hide:
                UIWindow.Close();
                break;

            case Action.GoBack:
                UIWindow.GoBack();
                break;
        }
    }
}
