using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvt_line_diag : MonoBehaviour {

    //pretty loose coding since this is a prototype

    private mvt_line_alt gutter2;
	int[] meshtopoly = new int[]{3,1,2,0};

	string dragPosition;

	bool isDragging;
	bool[] vertPulling;

	float lastMouse;

	// Use this for initialization
	void Awake () {
		dragPosition = "null";
		lastMouse = 0f;
        gutter2 = FindObjectOfType<mvt_line_alt>();
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
		Debug.Log (Mathf.Abs (mouse.x - top.x) / Mathf.Abs (bot.x - top.x));
		float placement = Mathf.Abs (mouse.x - top.x) / Mathf.Abs (bot.x - top.x);
		if (placement < 0.75f) {
			vertPulling = new bool[]{ true, true, true, true };
		}
	}

	//quad mesh points, from 
	void nudgePoints(bool[] p, float c){    //dragged game object and veriticies being pulled and amount to change them
        transform.position = new Vector3(Mathf.Clamp(transform.position.x+c,-4.5f,-0.9f), transform.position.y,transform.position.z);
        float x = Mathf.Clamp(gutter2.transform.GetChild(0).position.x + c, -2.3f, 0);
        gutter2.transform.GetChild(0).position = new Vector3(x,
                                                             gutter2.transform.GetChild(0).position.y,
                                                             gutter2.transform.GetChild(0).position.z);
        Debug.Log(x);
	}
}
