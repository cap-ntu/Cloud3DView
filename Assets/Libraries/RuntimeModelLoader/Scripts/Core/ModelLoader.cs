using UnityEngine;
using System.Collections;
using System.IO;

namespace RuntimeModelLoader {
	// Should be used wihtin this library only.
	public class ModelLoader {

		public static Mesh Load (string path) {
			if (!File.Exists(path)) return null;

			ParserBase parser = null;

			string extention = Path.GetExtension(path);
			switch (extention) {
			case ".obj":
				parser = new ObjParser();
				break;
			}

			if (!File.Exists (path)) {
				Debug.LogError ("Path does not exit: " + path);
			}
			Mesh mesh = parser.Parse(path);

			return mesh;
		}
	}

}