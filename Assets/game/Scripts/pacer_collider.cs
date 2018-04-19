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
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag == "pacer") {
					bool s = globalstate.Instance.getState (state);
					globalstate.Instance.setState (state, !s);
				}
			}
		}
	}

	public string getState(){
		return state;
	}
}
