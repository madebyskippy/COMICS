using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvt_line : MonoBehaviour {

	//pretty loose coding since this is a prototype

	int[] meshtopoly = new int[]{3,1,2,0};

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
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);
			if (hit.transform != null){
				if (hit.transform.tag == "gutter") {
					dragging = hit.transform.gameObject;
					dragPosition = hit.transform.name;
					lastMouse = Input.mousePosition.x;
				}
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

	//quad mesh points, from 
	void nudgePoints(GameObject d, bool[] p, float c){	//dragged game object and veriticies being pulled and amount to change them
		Mesh m = d.transform.parent.GetComponent<MeshFilter> ().mesh;
		Vector3[] v = m.vertices;
		PolygonCollider2D pc = d.GetComponent<PolygonCollider2D> ();
		Vector2[] pcv = pc.points;
		for (int i = 0; i < v.Length; i++) {
			if (p [i]) {
				//move it!!
				v[i] = new Vector3(v[i].x+c, v[i].y,v[i].z);
				pcv [meshtopoly [i]] = new Vector2 (pcv[meshtopoly[i]].x+c,pcv[meshtopoly[i]].y);
			}
		}
		m.vertices = v;
		pc.points = pcv;
	}
}
