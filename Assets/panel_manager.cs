using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_manager : MonoBehaviour {

	[SerializeField] GameObject heart;
	[SerializeField] GameObject tension;
	[SerializeField] GameObject cornerPanel;

	[SerializeField] mvt_line gutter1;
	[SerializeField] mvt_line_alt gutter2;
	[SerializeField] mvt_line_diag gutter3;

	// Use this for initialization
	void Start () {
		heart.SetActive (false);
		tension.SetActive (false);
		cornerPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		//if gutter 1 gone AND gutter 2 gone down, tension appears
		if (!gutter1.getOnScreen () && gutter2.getDown ()) {
			tension.SetActive (true);
		} else {
			tension.SetActive (false);
		}

		//if gutter 1 gone and gutter 2 still there, <3 appear
		if (!gutter1.getOnScreen () && !gutter2.getUp () && !gutter2.getDown()) {
			heart.SetActive (true);
		} else {
			heart.SetActive (false);
		}

		//if gutter 1 at max angle and gutter 2 not up, 3rd panel appear
		if (gutter1.getMaxAngle () && !gutter2.getUp ()) {
			cornerPanel.SetActive (true);
		} else {
			cornerPanel.SetActive (false);
		}
	}
}
