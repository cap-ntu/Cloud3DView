using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ModelLoaderFileExplorer {
	
	public class InputStorage : MonoBehaviour {
		public string key;

		private static Dictionary<string, string> _storage;


		void Awake () {
			if (_storage == null) {
				_storage = new Dictionary<string, string>();
			}
		}

		// This static method returns any input data saved in the storage.
		public static string GetValueForKey (string key) {
			string value;
			if (_storage.TryGetValue(key, out value)) {
				return value;
			}
			return null;
		}

		public void Submit (UIInput input) {
			string value;
			if (_storage.TryGetValue(key, out value)) {
				_storage[key] = input.value;
			}
			else {
				_storage.Add(key, input.value);
			}

			Debug.Log(_storage[key]);
		}
	}

}