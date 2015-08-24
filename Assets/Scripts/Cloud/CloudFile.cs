using UnityEngine;
using System;
using System.IO;
using System.Collections;

public static class CloudFile {

	public static IEnumerator DownloadFile (string filename, string url, Action callback) {
		var request = new WWW(url);
		yield return request;

		var parts = filename.Split('.');
		string localPath = Path.Combine(Application.persistentDataPath, "model." + parts[parts.Length-1]);
		var stream = File.Create(localPath);
		stream.Write(request.bytes, 0, request.bytes.Length);
		stream.Close();

		if (callback != null) {
			callback();
		}
	}
}
