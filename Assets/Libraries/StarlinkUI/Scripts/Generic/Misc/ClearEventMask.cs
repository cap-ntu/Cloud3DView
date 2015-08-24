using UnityEngine;

/// <summary>
/// Clear the event mask on the camera, ensuring that it will not be sending out any OnMouse events. This improves performance.
/// </summary>

[RequireComponent(typeof(Camera))]
public class ClearEventMask : MonoBehaviour
{
	void Start ()
	{
		camera.eventMask = 0;
	}
}
