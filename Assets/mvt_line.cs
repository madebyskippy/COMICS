using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvt_line : MonoBehaviour {

	//pretty loose coding since this is a prototype

	GameObject dragging;
	string dragPosition;

	float lastMouse;

	// Use this for initialization
	void Start () {
		dragging = null;
		dragPosition = "null";
		lastMouse = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				//assuming the only colliders in the scene are the gutter dragging points
				dragging = hit.transform.parent.gameObject;
				dragPosition = hit.transform.name;
				lastMouse = Input.mousePosition.x;
			} else {
				dragging = null;
				dragPosition = "null";
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			dragging = null;
			dragPosition = "null";
		}

		if (dragging != null) {
			bool[] pulling = new bool[4];
			if (dragPosition == "top") {
				pulling = new bool[]{ false, true, false, true };
			} else if (dragPosition == "mid") {
				pulling = new bool[]{ true, true, true, true };
			} else if (dragPosition == "bottom") {
				pulling = new bool[]{ true, false, true, false };
			}
			float change = Input.mousePosition.x - lastMouse;
			lastMouse = Input.mousePosition.x;
			nudgePoints (dragging, pulling, change);
		}
	}

	void nudgePoints(GameObject d, bool[] p, float c){	//dragged game object and veriticies being pulled and amount to change them
		Mesh m = d.GetComponent<MeshFilter> ().mesh;
		Vector3[] v = m.vertices;
		for (int i = 0; i < v.Length; i++) {
			if (p [i]) {
				//move it!!
				v[i] = new Vector3(v[i].x+c, v[i].y,v[i].z);
			}
		}
		m.vertices = v;
	}
}
