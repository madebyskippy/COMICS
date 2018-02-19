using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelmaker : MonoBehaviour {

	[SerializeField] LineRenderer panel;

	LineRenderer currentPanel;
	Vector3 topleft;

	// Use this for initialization
	void Start () {
		currentPanel = null;
		topleft = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			topleft = pos;
			currentPanel = Instantiate (panel, topleft, Quaternion.identity);
			currentPanel.SetPositions (new Vector3[] {topleft,topleft,topleft,topleft});
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Mathf.Abs (currentPanel.GetPosition (0).x - currentPanel.GetPosition (2).x) < 0.75f ||
			    Mathf.Abs (currentPanel.GetPosition (0).y - currentPanel.GetPosition (2).y) < 0.75f) {
				Debug.Log ("too small");
				Destroy (currentPanel);
			} else {
			}
			currentPanel = null;
		}

		if (currentPanel != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			currentPanel.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
			currentPanel.SetPosition (2, pos);
			currentPanel.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));
		}
	}
}
