using UnityEngine;
using System.Collections;

/// <summary>
/// This class makes it easy to delegate a WWW request.
/// Usage: WebRequest.Create(url, callback);
/// </summary>

public class GameWebRequest : MonoBehaviour
{
	public delegate void OnFinished (bool success, object param, string response);

	string mURL;
	OnFinished mCallback;
	object mParam;

	/// <summary>
	/// Start the download.
	/// </summary>

	IEnumerator Start ()
	{
		WWW www = null;

		try
		{
			www = new WWW(mURL);
		}
		catch (System.Exception ex)
		{
#if UNITY_EDITOR
			Debug.Log(ex.Message);
#endif
			if (mCallback != null) mCallback(false, mParam, ex.Message);
			www = null;
		}
		
		if (www != null)
		{
			yield return www;
			
			if (mCallback != null)
			{
				if (string.IsNullOrEmpty(www.error))
				{
					mCallback(true, mParam, www.text);
				}
				else
				{
					mCallback(false, mParam, www.error);
				}
			}
			www.Dispose();
			www = null;
		}
		Destroy(gameObject);
	}

	void End () { Destroy(gameObject); }

	/// <summary>
	/// Create a web request for the following URL, automatically cleaning up after it's done.
	/// </summary>

	static public void Create (string url) { Create(url, null, null); }

	/// <summary>
	/// Create a web request for the following URL.
	/// </summary>

	static public void Create (string url, OnFinished callback) { Create(url, callback, null); }

	/// <summary>
	/// Create a new web request for the following URL, providing the specified parameter.
	/// </summary>

	static public void Create (string url, OnFinished callback, object param)
	{
		if (Application.isPlaying)
		{
			GameObject go = new GameObject("_Game Web Request");
			DontDestroyOnLoad(go);

			GameWebRequest req = go.AddComponent<GameWebRequest>();
#if UNITY_EDITOR
			Debug.Log(url);
#endif
			req.mURL = url;
			req.mCallback = callback;
			req.mParam = param;
		}
	}
}
