using UnityEngine;

/// <summary>
/// Attach this component to a slider if you want it to control the music volume.
/// </summary>

[RequireComponent(typeof(UISlider))]
public class UIMusicVolume : MonoBehaviour
{
	UISlider mSlider;

	void Awake ()
	{
		mSlider = GetComponent<UISlider>();
		mSlider.value = Music.volume;
		EventDelegate.Add(mSlider.onChange, OnChange);
	}

	void OnChange ()
	{
		Music.volume = UISlider.current.value;
	}
}
