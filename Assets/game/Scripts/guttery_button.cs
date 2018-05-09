using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guttery_button : MonoBehaviour {

	[SerializeField] guttery_gutter gutter;
	[SerializeField] string row;

	tool_manager tm;

	// Use this for initialization
	void Start () {
		tm = GameObject.FindObjectOfType<tool_manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tm.getTool () != "gutter-button") {
			gutter.setActive (false);
		} else {
			if (tm.getActiveRow () != row) {
				gutter.setActive (false);
			}
		}
	}

	void OnMouseDown(){
		gutter.setActive (true);
	}
}
