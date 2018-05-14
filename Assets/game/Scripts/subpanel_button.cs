using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subpanel_button : MonoBehaviour {

	[SerializeField] Text countdown;

	Image buttonimg;

	// Use this for initialization
	void Start () {
		buttonimg = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (buttonimg.color.a < 0.5f) {
			countdown.enabled = false;
		} else {
			countdown.enabled = true;
		}
	}
}
