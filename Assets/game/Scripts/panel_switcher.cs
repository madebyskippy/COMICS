using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_switcher : MonoBehaviour {

	[SerializeField] panel_state[] states;
	[SerializeField] Image image; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < states.Length; i++) {
			if (states [i].checkState ()) {
				image.sprite = states [i].getSprite ();
			}
		}
	}


}
