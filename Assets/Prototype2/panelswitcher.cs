using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Group {bed, clothes, piano, chair, door, phone};

public class panelswitcher : MonoBehaviour {

	public Group group;
	public bool isIn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter(){
		isIn = true;
	}

	void OnMouseOver(){
		isIn = true;
	}

	void OnMouseExit(){
		isIn = false;
	}
}
