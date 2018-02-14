﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvt_line_alt : MonoBehaviour {

    //pretty loose coding since this is a prototype

    private mvt_line gutter1;
	int[] meshtopoly = new int[]{3,1,2,0};

	string dragPosition;

	bool isDragging;
	bool[] vertPulling;

	float lastMouse;

	// Use this for initialization
	void Start () {
		dragPosition = "null";
		lastMouse = 0f;
        gutter1 = FindObjectOfType<mvt_line>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isDragging) {
			float change = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - lastMouse;
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
			nudgePoints (vertPulling, change);
		}
	}

	void OnMouseDown(){
		if (!isDragging) {
			//determine position...
			lastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
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
		if (placement < 0.75f) {
			vertPulling = new bool[]{ true, true, true, true };
		}
	}

	//quad mesh points, from 
	void nudgePoints(bool[] p, float c){    //dragged game object and veriticies being pulled and amount to change them

        float scaleY = gutter1.transform.localScale.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + c + transform.position.z);
        Mesh m = gutter1.GetComponent<MeshFilter> ().mesh;
		Vector3[] v = m.vertices;
		PolygonCollider2D pc = gutter1.GetComponent<PolygonCollider2D> ();
		Vector2[] pcv = pc.points;


        for (int i = 0; i < v.Length; i++) {
            if (i == 0 || i == 2) //bottom two vertices
            {
                v[i] = new Vector3(v[i].x, v[i].y + c/scaleY, v[i].z);
                 pcv[meshtopoly[i]] = new Vector2(pcv[meshtopoly[i]].x, pcv[meshtopoly[i]].y + c/scaleY);
            }
		}
		m.vertices = v;
		pc.points = pcv;
	}
}
