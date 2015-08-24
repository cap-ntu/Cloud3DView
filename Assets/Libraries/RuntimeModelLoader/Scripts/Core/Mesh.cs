using System.Collections;
using System.Collections.Generic;

namespace RuntimeModelLoader {

	public class Mesh {
		public List<Vec3> vertices {get; set;}
		public List<Face> faces {get; set;}
		public List<Vec3> normals {get; set;}

		public Mesh () {
			vertices = new List<Vec3>();
			faces = new List<Face>();
			normals = new List<Vec3>();
		}
	}

	public struct Vec3 {
		public float x;
		public float y;
		public float z;

		public Vec3 (float x, float y, float z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString () {
			return string.Format ("[{0}, {1}, {2}]", x, y, z);
		}
	}

	public struct Face {
		public int[] vertexIndices;
		public int[] cvIndices;
		public int[] normalIndices;

		public Face (int verticesCount) {
			vertexIndices = new int[verticesCount];
			cvIndices = new int[verticesCount];			// As: f 	v/vt/vn		v/vt/vn ...
			normalIndices = new int[verticesCount];
		}

		public override string ToString ()
		{
			return string.Format ("[{0}, {1}, {2}]", vertexIndices[0], vertexIndices[1], vertexIndices[2]);
		}
	}

}