using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_switcher : MonoBehaviour {

	[SerializeField] panel_state[] states;
	[SerializeField] Image image; 

	int currentState;
	pageturner pt;
	int page;

	// Use this for initialization
	void Start () {
		pt = GameObject.FindObjectOfType<pageturner> ();
		page = (int)(transform.parent.name.ToCharArray () [2])-49;
		currentState = 0;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < states.Length; i++) {
			if (states [i].checkState ()) {
				if (i != currentState){
					currentState = i;
					image.sprite = states [i].getSprite ();
					pt.markUnread (page);
				}
			}
		}
	}


}
