using UnityEngine;
using System.Collections;
using System.IO;

namespace RuntimeModelLoader {

	public class ParserBase {
		protected Mesh _mesh;

		public virtual Mesh Parse (string path) {return null;}
	}

	public static class Settings {
		public static bool faceStartFromZero = true;

	}
}