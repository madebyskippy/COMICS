using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subpanel_collider : MonoBehaviour {

	[SerializeField] Sprite img;
	[SerializeField] string state;

	// Use this for initialization
	void Start () {
		
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
}
