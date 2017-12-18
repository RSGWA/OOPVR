using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
public class MeshGenerator : MonoBehaviour {

	void Start () {
		CreateCube ();
	}

	private void CreateCube () {
		Vector3[] vertices = {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 1, 0),
			new Vector3 (0, 1, 0),
			new Vector3 (0, 1, 1),
			new Vector3 (1, 1, 1),
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
			new Vector3 (0, 0.5f, 0),
			new Vector3 (0.35f, 0.5f, 0),
			new Vector3 (0.65f, 0.5f, 0),
			new Vector3 (1, 0.5f, 0),
			new Vector3 (0.35f, 0, 0),
			new Vector3 (0.65f, 0, 0),
		};

		int[] triangles = {
			//0, 2, 1, //face front
			//0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			//0, 6, 7, //face bottom
			//0, 1, 6,
			8, 3, 2,
			8, 2, 11,
			0, 8, 9,
			0, 9, 12,
			13, 10, 11,
			13, 11, 1
		};
			
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	}
}