using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guttery_button : MonoBehaviour {

	[SerializeField] guttery_gutter gutter;
	[SerializeField] string row;
	[SerializeField] GameObject[] targets;

	tool_manager tm;

	// Use this for initialization
	void Start () {
		tm = GameObject.FindObjectOfType<tool_manager> ();
		triggerTargets (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (tm.getTool () != "gutter-button") {
			gutter.setActive (false);
			triggerTargets (false);
		} else {
			if (tm.getActiveRow () != row) {
				gutter.setActive (false);
				triggerTargets (false);
			}
		}
	}

	void OnMouseDown(){
		gutter.setActive (true);
		triggerTargets (true);
	}

	void triggerTargets(bool b){
		for (int i = 0; i < targets.Length; i++) {
			targets [i].SetActive (b);
		}
	}
}
