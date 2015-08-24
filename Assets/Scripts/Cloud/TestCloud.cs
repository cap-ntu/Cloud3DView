using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVOSCloud;
using Parse;

public class TestCloud : MonoBehaviour {

	private bool _submitted;
	

	void LateUpdate () {
		if (_submitted) return ;

		StartCoroutine(DownloadFileP());
//		UploadFileP();

		_submitted = true;
	}

	private void Query () {
		var model = new AVObject ("Model1");
		model ["name"] = "TestModel";
		model ["cpucount"] = 3;
		model.SaveAsync().ContinueWith(t=>{
			if (!t.IsFaulted) {
				Debug.Log("Success!");
			}
			else {
				Debug.Log("Error!");
			}

			var query = new AVQuery<AVObject>("Model1");
			return query.GetAsync("551d1952e4b062519520cf96");
		}).Unwrap().ContinueWith(t => {
			var modelGot = t.Result;
			Debug.Log(modelGot.Get<string>("name"));
			Debug.Log(modelGot.Get<int>("cpucount"));
		});
	}

	private void RegisterUser () {
		var user = new AVUser();
		user.Username = "Yifeng";
		user.Password = "12345678";
		user.Email = "f15gdsy@gmail.com";
		user.SignUpAsync().ContinueWith(t => {
			if (t.IsFaulted) {
				Debug.Log("Register Failed");
			}
			else {
				Debug.Log("Register Succeeded");
			}
		});
	}

	private void UploadFile () {
//		var file = AVFile.CreateFileWithLocalPath("character.png", 
//		                                          System.IO.Path.Combine(Application.persistentDataPath, "character.png"));

		var file = AVFile.CreateFileWithLocalPath("Hello.txt", 
		                                          System.IO.Path.Combine(Application.persistentDataPath, "Hello.txt"));
		file.SaveAsync();
	}

	private void UploadFileP () {
		string path = System.IO.Path.Combine(Application.persistentDataPath, "cube.obj");
		var file = new ParseFile("cube.obj", File.ReadAllBytes(path));

		var data = new ParseObject("TestFile");
		data["name"] = "cube 1";
		data["file"] = file;

		data.SaveAsync().ContinueWith(t => {
			if (t.IsFaulted || t.IsCanceled) {
				Debug.Log("Upload Failed");
			}
			else {
				Debug.Log("Upload Success");
			}
		});
	}

	private void DownloadFile () {
		AVFile.GetFileWithObjectIdAsync("551d7e98e4b06251952588b1").ContinueWith(t => {
			var file = t.Result;
			file.DownloadAsync().ContinueWith(s => {
				var dataByte = file.DataByte;
				Debug.Log(dataByte);
			});
		});
	}

	IEnumerator DownloadFileP () {
		var query = new ParseQuery<ParseObject>("TestFile");
		query.Include("file");
		var t = query.GetAsync("UD2rshu4aq");
		while (!t.IsCompleted) {
			yield return null;
		}

		var data = t.Result;
		var filename = data.Get<string>("name");
		var file = data.Get<ParseFile>("file");

		var fileRequest = new WWW(file.Url.AbsoluteUri);
		yield return fileRequest;
		Debug.Log(filename + ": " + fileRequest.text);

		var parts = filename.Split('.');
		string localPath = Path.Combine(Application.persistentDataPath, "model." + parts[parts.Length-1]);
		var stream = File.Create(localPath);
		stream.Write(fileRequest.bytes, 0, fileRequest.bytes.Length);
	}



	private void QueryList () {
		var data = new AVObject("List");
		data["name"] = "hello";
		data["Parameters"] = new List<int>(new int[] {3, 2, 1});

		data.SaveAsync().ContinueWith(t => {
			if (t.IsFaulted || t.IsCanceled) {
				Debug.Log("Failed");
				Debug.Log(t.Exception.Message);
			}
			else {
				Debug.Log("Success");
			}
		}).ContinueWith(t => {
			var query = new AVQuery<AVObject>("List").FindAsync().ContinueWith(t2 => {
				foreach (var result in t2.Result) {
//					var hello = result.Get<string>("name");
//					Debug.Log(hello);
					var list = result.Get<IList<int>>("Parameters");
					Debug.Log(list.ToString());
				}

			});
		});
	}

	private class IntPair {
		int a;
		int b;
		public IntPair(int a, int b) {
			this.a = a;
			this.b = b;
		}
	}

	public enum CloudTestOpt {
		Data,
		User,
		File,
	}
}
