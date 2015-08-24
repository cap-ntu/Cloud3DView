using UnityEngine;
using System.Collections;
using System.IO;

namespace RuntimeModelLoader {

	public class ObjParser : ParserBase {

		private string[] _tokens;
		

		public override Mesh Parse (string path) {
			_mesh = new Mesh();

			using (StreamReader reader = new StreamReader(path)) {

				string line;

				while (null != (line = reader.ReadLine())) {
					if (string.IsNullOrEmpty(line)) {
						continue;
					}

					_tokens = line.Split(new []{' ', '\t'}, System.StringSplitOptions.RemoveEmptyEntries);
					switch (_tokens[0]) {
					case "v":
						GetVertex3();
						break;
					case "vt":
						break;
					case "vn":
						break;
					case "f":
						GetFace();
						break;
					}
				}
			}


			return _mesh;
		}

		private void GetVertex3 () {
			float x = float.Parse(_tokens[1]);
			float y = float.Parse(_tokens[2]);
			float z = float.Parse(_tokens[3]);
			Vec3 vec3 = new Vec3(x, y, z);

			_mesh.vertices.Add(vec3);
		}

		private void GetFace () {
			int verticesCount = _tokens.Length - 1;

//			if (verticesCount > 3) {
//				Debug.LogError("Obj Parser: only support triangular faces");
//			}

			Face face = new Face(verticesCount);

			if (verticesCount > 3) {
				Debug.Log("HHHHHHHH");
			}

			for (int i=1; i<=verticesCount; i++) {
				string[] ids = _tokens[i].Split(new []{"/"}, System.StringSplitOptions.RemoveEmptyEntries);

				int vId = int.Parse(ids[0]);
				vId = Settings.faceStartFromZero ? vId-1 : vId;

				// TODO: handle normal & texutre coordinate
				int vtId = -1;
				int vnId = -1;

				if (ids.Length > 2) {			// Handles: f v/vt/vn 
					if (!string.IsNullOrEmpty(ids[1])) {	// In case: f v//vn
						vtId = int.Parse(ids[1]);
					}

					vnId = int.Parse(ids[2]);
				}
				else if (ids.Length > 1) {		// Handles: f v/vt
					vtId = int.Parse(ids[1]);
				}

				face.vertexIndices[i-1] = vId;
				face.cvIndices[i-1] = vtId;
				face.normalIndices[i-1] = vnId;
			}

			_mesh.faces.Add(face);
		}
	}

}