using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mvt_line : MonoBehaviour {

	//pretty loose coding since this is a prototype

	int[] meshtopoly = new int[]{3,1,2,0}; //poly order is top left, top right, bot right, bot left... mesh order is weird

	string dragPosition;

	float gutterwidth;
	Mesh guttermesh;
	PolygonCollider2D guttercollider;

	bool isDragging;
	bool[] vertPulling;

	float lastMouse;

	bool isMaxAngle;
	bool isOnScreen;

	// Use this for initialization
	void Awake () {
		dragPosition = "null";
		lastMouse = 0f;
		isMaxAngle = false;
		isOnScreen = true;
		guttermesh = transform.GetComponent<MeshFilter> ().mesh;
		guttercollider = GetComponent<PolygonCollider2D> ();

		Vector3[] v = guttermesh.vertices;
		gutterwidth = v [2].x - v [0].x;
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
//		Debug.Log (Mathf.Abs (mouse.y - top.y) / Mathf.Abs (bot.y - top.y));
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
		Vector3[] v = guttermesh.vertices;
		Vector2[] pcv = guttercollider.points;

		//check bounds
		float xbot = v[0].x + c * Convert.ToInt32(p[0]); //bottom left
		float xtop = v [3].x + c * Convert.ToInt32 (p [3]); //top left
		float height = v[3].y-v[0].y;
		float width = Mathf.Abs(xtop - xbot);

		float limit = height * Mathf.Tan (Mathf.Deg2Rad * 70f);
		if (width >= (limit*0.99f)) { //to give it a little wiggle room because otherwise it was erroring
			isMaxAngle = (xtop > xbot); // only true if it's slanted to the right
			if (p [0] && !p [3]) {
				//moving the bottom and not the top
				//clamp vert 0 to height*tan(70) from the top x
				xbot = xtop + limit * (Convert.ToInt32 (xtop < xbot) * 2 - 1); //1 or -1 depending on if it was greater or less
				Debug.Log ("moving bot");
			} else if (!p [0] && p [3]) {
				//moving the top and not the bottom
				//clamp vert 3
				xtop = xbot + limit * (Convert.ToInt32 (xtop > xbot) * 2 - 1); //1 or -1 depending on if it was greater or less
				Debug.Log ("moving top");
			}
		} else {
			isMaxAngle = false;
		}

		xbot = Mathf.Clamp (xbot, -6.4f, 5.2f);
		xtop = Mathf.Clamp (xtop, -6.4f, 5.2f);
		if ((xbot <= -6.4f || xtop <= -6.4f) || (xbot >= 5.2f || xtop >= 5.2f)) {
			isOnScreen = false;
		} else {
			isOnScreen = true;
		}

		//move it!!
		v[0] = new Vector3(xbot, v[0].y,v[0].z);
		pcv [meshtopoly [0]] = new Vector2 (xbot,pcv[meshtopoly[0]].y);

		v[1] = new Vector3(xtop+gutterwidth, v[1].y,v[1].z);
		pcv [meshtopoly [1]] = new Vector2 (xtop+gutterwidth,pcv[meshtopoly[1]].y);

		v[2] = new Vector3(xbot+gutterwidth, v[2].y,v[2].z);
		pcv [meshtopoly [2]] = new Vector2 (xbot+gutterwidth,pcv[meshtopoly[2]].y);

		v[3] = new Vector3(xtop, v[3].y,v[3].z);
		pcv [meshtopoly [3]] = new Vector2 (xtop,pcv[meshtopoly[3]].y);

		guttermesh.vertices = v;
		guttercollider.points = pcv;
	}

	public bool getOnScreen(){
		return isOnScreen;
	}

	public bool getMaxAngle(){
		return isMaxAngle;
	}
}
