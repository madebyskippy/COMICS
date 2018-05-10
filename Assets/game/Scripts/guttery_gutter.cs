﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guttery_gutter : MonoBehaviour {

	[SerializeField] Transform[] snapTransforms;
	Vector3[] snapPos;

	[Space(20)]
	[SerializeField] string stateStraight;

	[Header("only used if it's diagonal")]
	[SerializeField] bool isDiagonal;
	[SerializeField] string stateLeftSlant;
	[SerializeField] string stateRightSlant;

	[Space(20)]
	[SerializeField] Transform minPos;
	[SerializeField] Transform maxPos;

	[Space(20)]
	[SerializeField] Material activeColor;
	Material inactiveColor;

	[Space(20)]
	[SerializeField] guttery_button button;

	int[] meshtopoly = new int[]{3,1,2,0}; //poly order is top left, top right, bot right, bot left... mesh order is weird

	MeshRenderer mr;
	float gutterwidth;
	Mesh guttermesh;
	PolygonCollider2D guttercollider;

	Vector2 gutterrange;

	bool isDragging = false;
	bool isTop = false; //used for telling which diagonal end you're pulling

	tool_manager tm;

	bool isActive;

	// Use this for initialization
	void Start () {
		tm = GameObject.FindObjectOfType<tool_manager> ();
		mr = GetComponent<MeshRenderer> ();
		inactiveColor = mr.material;
		guttermesh = transform.GetComponent<MeshFilter> ().mesh;
		guttercollider = GetComponent<PolygonCollider2D> ();
		Vector3[] v = guttermesh.vertices;
		gutterwidth = v [2].x - v [0].x;
		float min = transform.InverseTransformPoint (minPos.position).x;
		float max = transform.InverseTransformPoint (maxPos.position).x;
		gutterrange = new Vector2 (min, max);
		snapPos = new Vector3[snapTransforms.Length];
		for (int i = 0; i < snapPos.Length; i++) {
			snapPos [i] = transform.InverseTransformPoint (snapTransforms [i].position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos = transform.InverseTransformPoint (pos);
			nudgePoints (pos.x);

			if (!isDiagonal || (isDiagonal && isTop)) {
				Vector3 mousep = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				button.transform.position = new Vector3 (mousep.x, button.transform.position.y, button.transform.position.z);
			}
		}
	}

	void OnMouseDown(){
		if (isActive) {
			if (!isDragging) {
				if (isDiagonal) {
					getClickPosition (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				}
			}
			isDragging = true;
		}

	}void OnMouseUp(){
		isDragging = false;
		Vector3 closest = snapPos[0];
		float distance = 100000000f;
		for (int i=0; i<snapPos.Length; i++){
			float d = Mathf.Abs(Vector2.Distance(new Vector2(snapPos[i].x,snapPos[i].y),
				new Vector2(button.transform.position.x,button.transform.position.y)));
			if (d < distance){
				closest = snapPos[i];
				distance = d;
			}
		}
		nudgePoints(closest.x);
		Vector3 pos = transform.TransformPoint(closest);
		button.transform.position = new Vector3 (pos.x, button.transform.position.y, button.transform.position.z);
	}

	void getClickPosition(Vector3 mouse){
		Vector3 top = transform.TransformPoint (transform.GetComponent<MeshFilter> ().mesh.vertices [3]);
		Vector3 bot = transform.TransformPoint (transform.GetComponent<MeshFilter> ().mesh.vertices [0]);
		//		Debug.Log (Mathf.Abs (mouse.y - top.y) / Mathf.Abs (bot.y - top.y));
		float placement = Mathf.Abs (mouse.y - top.y) / Mathf.Abs (bot.y - top.y);
		if (placement < 0.25f) {
			//top
			isTop = true;
		} else if (placement < 0.75f) {
			//mid
		} else {
			//bot
			isTop = false;
		}
	}

	void nudgePoints(float c){
		Vector3[] v = guttermesh.vertices;
		Vector2[] pcv = guttercollider.points;

		float xbot = c-gutterwidth/2;
		float xtop = c-gutterwidth/2;

		if (isDiagonal) {
			if (isTop) {
				xbot = v [0].x;
			} else {
				xtop = v [3].x;
			}
		}

		xbot = Mathf.Clamp (xbot, gutterrange.x-gutterwidth,gutterrange.y);
		xtop = Mathf.Clamp (xtop, gutterrange.x-gutterwidth,gutterrange.y);

		v [0] = new Vector3 (xbot, v [0].y, v [0].z);
		pcv [meshtopoly [0]] = new Vector2 (xbot, pcv [meshtopoly [0]].y);

		v[2] = new Vector3(xbot+gutterwidth, v[2].y,v[2].z);
		pcv [meshtopoly [2]] = new Vector2 (xbot+gutterwidth,pcv[meshtopoly[2]].y);

		v [1] = new Vector3 (xtop + gutterwidth, v [1].y, v [1].z);
		pcv [meshtopoly [1]] = new Vector2 (xtop + gutterwidth, pcv [meshtopoly [1]].y);

		v [3] = new Vector3 (xtop, v [3].y, v [3].z);
		pcv [meshtopoly [3]] = new Vector2 (xtop, pcv [meshtopoly [3]].y);

		if (!isDiagonal) {
			//switch state when it hits the ends
			if (xbot == gutterrange.x - gutterwidth || xbot == gutterrange.y) {
				globalstate.Instance.setState (stateStraight, false);
			} else {
				globalstate.Instance.setState (stateStraight, true);
			}
		} else {
			if (v [0].x > v [3].x) {
				//bottom > top, this shape \
				globalstate.Instance.setState (stateLeftSlant, true);
				globalstate.Instance.setState (stateRightSlant, false);
			} else if (v [0].x < v [3].x) {
				//top > bottom, this shape /
				globalstate.Instance.setState (stateRightSlant, true);
				globalstate.Instance.setState (stateLeftSlant, false);
			}
		}

		guttermesh.vertices = v;
		guttercollider.points = pcv;
	}

	public void setActive(bool b){
		isActive = b;
		if (isActive) {
			mr.material = activeColor;
		} else {
			mr.material = inactiveColor;
		}
	}
}
