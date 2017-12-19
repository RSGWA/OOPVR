using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceController : MonoBehaviour {
	
	public GameObject player;
	public Material transparent;
	public Material tinted;

	// Use this for initialization
	void Start () {
		// CreateCubeWithDoor ();
		makeTinted ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createInstance() {
		// Create instance somehow
		this.gameObject.SetActive(true);
	}

	public void movePlayer() {
		player.transform.position = new Vector3 (transform.localPosition.x + (transform.localScale.x * 0.25f), player.transform.position.y, transform.localPosition.z);
		GetComponent<BoxCollider> ().enabled = false;
		reverseNormals ();
		makeTransparent ();
		Debug.Log ("PLAYER MOVED");
	}

	void makeTransparent() {
		GetComponent<Renderer> ().material = transparent;
	}

	void makeTinted() {
		GetComponent<Renderer> ().material = tinted;
	}

	void reverseNormals() {
		MeshFilter filter = GetComponent(typeof (MeshFilter)) as MeshFilter;
		if (filter != null) {
			Mesh mesh = filter.mesh;

			Vector3[] normals = mesh.normals;
			for (int i=0;i<normals.Length;i++)
				normals[i] = -normals[i];
			mesh.normals = normals;

			for (int m=0;m<mesh.subMeshCount;m++) {
				int[] triangles = mesh.GetTriangles(m);
				for (int i=0;i<triangles.Length;i+=3) {
					int temp = triangles[i + 0];
					triangles[i + 0] = triangles[i + 1];
					triangles[i + 1] = temp;
				}
				mesh.SetTriangles(triangles, m);
			}
		}
		Debug.Log ("NORMALS REVERSED");
	}

	private void CreateCubeWithDoor () {
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