using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RuntimeModelLoader;


namespace RuntimeModelLoader.Ex {

	public class ModelEx : MonoBehaviour {

		public string path;
		public bool loadOnAwake;


		private MeshFilter _meshFilter;
//		private Renderer _renderer;

		void Awake () {
			_meshFilter = GetComponent<MeshFilter>();
//			_renderer = renderer;

			if (loadOnAwake) {
				Load();
			}
		}
		
		public bool Load () {
			RuntimeModelLoader.Mesh runtimeMesh = ModelLoader.Load(path);
			if (runtimeMesh == null) {return false;}
			
			List<Vector3> vertices = Converter.GetVerticesVector3(runtimeMesh);
			List<int> triangles = Converter.GetTriangules(runtimeMesh);

			UnityEngine.Mesh goMesh = new UnityEngine.Mesh();
			goMesh.vertices = vertices.ToArray();
			goMesh.triangles = triangles.ToArray();

			_meshFilter.mesh = goMesh;

			return true;
		}
	}

}