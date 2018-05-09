using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subpanel_collider : MonoBehaviour {

	[SerializeField] Sprite img;
	[SerializeField] string state;

	BoxCollider2D col;

	// Use this for initialization
	void Start () {
		col = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string getState(){
		return state;
	}

	public Sprite getImg(){
		return img;
	}

	public void setColliderState(bool s){
		if (s != col.enabled) {
			col.enabled = s;
		}
	}
}
