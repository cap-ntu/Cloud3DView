using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuntimeModelLoader;

namespace RuntimeModelLoader.Ex {

	public class ModelLoaderEx {

		public static GameObject LoadModelGameObject (string path, Material material = null) {
			RuntimeModelLoader.Mesh runtimeMesh = ModelLoader.Load(path);
			if (runtimeMesh == null) {return null;}

			List<Vector3> vertices = Converter.GetVerticesVector3(runtimeMesh);
			List<int> triangles = Converter.GetTriangules(runtimeMesh);

			GameObject go = new GameObject();

			UnityEngine.Mesh goMesh = new UnityEngine.Mesh();
			goMesh.vertices = vertices.ToArray();
			goMesh.triangles = triangles.ToArray();

			MeshFilter meshFilter = go.AddComponent<MeshFilter>();
			meshFilter.mesh = goMesh;

			MeshRenderer renderer = go.AddComponent<MeshRenderer>();
			renderer.material = material;

			return go;
		}
	}


	public static class Converter {
		public static Vector3 Vec3ToVector3 (Vec3 vec3) {
			return new Vector3(vec3.x, vec3.y, vec3.z);
		}

		public static List<Vector3> GetVerticesVector3 (RuntimeModelLoader.Mesh mesh) {
			List<Vector3> vertices = new List<Vector3>();
			foreach (Vec3 vec3 in mesh.vertices) {
				Vector3 vector3 = Converter.Vec3ToVector3(vec3);
				vertices.Add(vector3);
			}
			return vertices;
		}

		public static List<int> GetTriangules (RuntimeModelLoader.Mesh mesh) {
			List<int> triangles = new List<int>();
			foreach (Face face in mesh.faces) {
				if (face.vertexIndices.Length == 3) {
					triangles.Add(face.vertexIndices[0]);
					triangles.Add(face.vertexIndices[1]);
					triangles.Add(face.vertexIndices[2]);
				}
				else if (face.vertexIndices.Length == 4) {
					triangles.Add(face.vertexIndices[0]);
					triangles.Add(face.vertexIndices[1]);
					triangles.Add(face.vertexIndices[2]);

					triangles.Add(face.vertexIndices[0]);
					triangles.Add(face.vertexIndices[2]);
					triangles.Add(face.vertexIndices[3]);
				}
			}
			return triangles;
		}
	}
}