using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceController : MonoBehaviour {
	
	public GameObject player;
	public Material transparent;
	public Material tinted;

	private Animator anim;

	// Use this for initialization
	void Start () {
		// CreateCubeWithDoor ();
		//this.gameObject.SetActive(false);
		//makeTransparent ();
		anim = GetComponent<Animator> ();
		anim.speed = 0.55f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void createInstance() {
		this.gameObject.SetActive(true);
		InstanceControl ("Create");
	}

	void InstanceControl(string direction) {
		anim.SetTrigger(direction);
	}

	public void makeTransparent() {
		GetComponent<Renderer> ().material = transparent;
	}

	public void makeTinted() {
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
}