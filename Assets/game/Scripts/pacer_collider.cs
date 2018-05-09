using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacer_collider : MonoBehaviour {

	[SerializeField] string state;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseDown(){	
		bool s = globalstate.Instance.getState (state);
		globalstate.Instance.setState (state, !s);
	}

	public string getState(){
		return state;
	}
}
