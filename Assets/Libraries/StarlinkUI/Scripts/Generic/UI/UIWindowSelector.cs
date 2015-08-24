using UnityEngine;

/// <summary>
/// This class is used to open up the correct window once the player plays the game then returns to the menu.
/// Without this class, it will be the same as restarting the game. With this class, the "new game" window
/// will be shown instead, letting the player start another game without having to navigate through the menus.
/// </summary>

public class UIWindowSelector : MonoBehaviour
{
	static bool mFirstTime = true;

	public UIPanel gameType;
	public UIPanel newGame;

	/// <summary>
	/// Triggered when re-entering the main menu (likely from playing a game)
	/// </summary>

	void Start ()
	{
		if (!mFirstTime)
		{
			UIWindow.Show(gameType);
			UIWindow.Show(newGame);
		}
		mFirstTime = false;
	}
}
