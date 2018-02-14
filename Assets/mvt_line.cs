using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mvt_line : MonoBehaviour {

	//pretty loose coding since this is a prototype

	int[] meshtopoly = new int[]{3,1,2,0}; //poly order is top left, top right, bot right, bot left... mesh order is weird

	string dragPosition;

	bool isDragging;
	bool[] vertPulling;

	float lastMouse;

	// Use this for initialization
	void Awake () {
		dragPosition = "null";
		lastMouse = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging) {
			float change = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - lastMouse;
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			nudgePoints (vertPulling, change);
		}
	}

	void OnMouseDown(){
		if (!isDragging) {
			//determine position...
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			getClickPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
		isDragging = true;

	}void OnMouseUp(){
		isDragging = false;
	}

	void getClickPosition(Vector3 mouse){
		Vector3 top = transform.TransformPoint (transform.GetComponent<MeshFilter> ().mesh.vertices [3]);
		Vector3 bot = transform.TransformPoint (transform.GetComponent<MeshFilter> ().mesh.vertices [0]);
		Debug.Log (Mathf.Abs (mouse.y - top.y) / Mathf.Abs (bot.y - top.y));
		float placement = Mathf.Abs (mouse.y - top.y) / Mathf.Abs (bot.y - top.y);
		if (placement < 0.25f) {
			//top
			vertPulling = new bool[]{ false, true, false, true };
			dragPosition = "top";
		} else if (placement < 0.75f) {
			//mid
			vertPulling = new bool[]{ true, true, true, true };
			dragPosition = "mid";
		} else {
			//bot
			vertPulling = new bool[]{ true, false, true, false };
			dragPosition = "bottom";
		}
	}

	//quad mesh points, from 
	void nudgePoints(bool[] p, float c){	//dragged game object and veriticies being pulled and amount to change them
		Mesh m = transform.GetComponent<MeshFilter> ().mesh;
		Vector3[] v = m.vertices;
		PolygonCollider2D pc = GetComponent<PolygonCollider2D> ();
		Vector2[] pcv = pc.points;

		//check bounds
		float xbot = v[0].x + c * Convert.ToInt32(p[0]); //bottom left
		float xtop = v [3].x + c * Convert.ToInt32 (p [3]); //top left
		float height = v[3].y-v[0].y;
		float width = Mathf.Abs(xtop - xbot);
		if (width > height * Mathf.Tan(Mathf.Deg2Rad*70f)){
			return;
		}

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
