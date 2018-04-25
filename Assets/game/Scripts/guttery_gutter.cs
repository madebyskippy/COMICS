﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guttery_gutter : MonoBehaviour {

	[SerializeField] string stateStraight;

	[Header("only used if it's diagonal")]
	[SerializeField] bool isDiagonal;
	[SerializeField] string stateLeftSlant;
	[SerializeField] string stateRightSlant;

	[Space(20)]
	[SerializeField] Transform minPos;
	[SerializeField] Transform maxPos;

	int[] meshtopoly = new int[]{3,1,2,0}; //poly order is top left, top right, bot right, bot left... mesh order is weird

	float gutterwidth;
	Mesh guttermesh;
	PolygonCollider2D guttercollider;

	Vector2 gutterrange;

	bool isDragging = false;
	float lastMouse;

	// Use this for initialization
	void Start () {
		guttermesh = transform.GetComponent<MeshFilter> ().mesh;
		guttercollider = GetComponent<PolygonCollider2D> ();
		Vector3[] v = guttermesh.vertices;
		gutterwidth = v [2].x - v [0].x;
		float min = transform.InverseTransformPoint (minPos.position).x;
		float max = transform.InverseTransformPoint (maxPos.position).x;
		gutterrange = new Vector2 (min, max);
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging) {
			float change = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - lastMouse;
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
			nudgePoints (change);
//			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			pos.x = Mathf.Clamp (pos.x, minPos.position.x, maxPos.position.x);
//			transform.position = new Vector3 (pos.x, transform.position.y, transform.position.z);
		}
	}

	void OnMouseDown(){
		if (!isDragging) {
			//determine position...
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
		}
		isDragging = true;

	}void OnMouseUp(){
		isDragging = false;
	}

	void nudgePoints(float c){
		Vector3[] v = guttermesh.vertices;
		Vector2[] pcv = guttercollider.points;

		float xbot = v [0].x + c;
		float xtop = v [3].x + c;

		xbot = Mathf.Clamp (xbot, gutterrange.x-gutterwidth,gutterrange.y);
		xtop = Mathf.Clamp (xtop, gutterrange.x-gutterwidth,gutterrange.y);

		if (!isDiagonal) {
			//switch state when it hits the ends
			if (xbot == gutterrange.x - gutterwidth || xbot == gutterrange.y) {
				globalstate.Instance.setState (stateStraight, false);
			} else {
				globalstate.Instance.setState (stateStraight, true);
			}
		} else {
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
}