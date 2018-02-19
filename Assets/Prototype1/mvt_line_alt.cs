using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvt_line_alt : MonoBehaviour {

    //pretty loose coding since this is a prototype

    private mvt_line gutter1;
    private mvt_line_diag gutter3;
	int[] meshtopoly = new int[]{3,1,2,0};

	string dragPosition;

	bool isDragging;
	bool[] vertPulling;

	float lastMouse;

    private float y;

	bool isUp;
	bool isDown;

	// Use this for initialization
	void Awake () {
		dragPosition = "null";
		lastMouse = 0f;
        gutter1 = FindObjectOfType<mvt_line>();
        gutter3 = FindObjectOfType<mvt_line_diag>();
        y = transform.position.y;
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
    void nudgePoints(bool[] p, float c)
    {    //dragged game object and veriticies being pulled and amount to change them

        float scaleY = gutter1.transform.localScale.y;
        y = Mathf.Clamp(transform.position.y + c, -4.76f, 4.75f);
        transform.position = new Vector3(transform.position.x,
                                         y,
                                         transform.position.z);


		isUp = (y >= 4.75);
		isDown = (y <= -4.76);

        if (isDown || isUp)//!(-4.76f <= y && y <= 4.75f))
        {
            return;
        }


        gutter3.transform.position = new Vector3(gutter3.transform.position.x,
                                                 gutter3.transform.position.y + c,
                                                 gutter3.transform.position.z);

        Mesh m = gutter1.GetComponent<MeshFilter>().mesh;
        Vector3[] v = m.vertices;
        PolygonCollider2D pc = gutter1.GetComponent<PolygonCollider2D>();
        Vector2[] pcv = pc.points;

        for (int i = 0; i < v.Length; i++)
        {
            if (i == 0 || i == 2) //bottom two vertices
            {
                v[i] = new Vector3(v[i].x, v[i].y + c / scaleY, v[i].z);
                pcv[meshtopoly[i]] = new Vector2(pcv[meshtopoly[i]].x, pcv[meshtopoly[i]].y + c / scaleY);
            }
        }
        m.vertices = v;
        pc.points = pcv;

    }

	public bool getUp(){
		return isUp;
	}

	public bool getDown(){
		return isDown;
	}
}
