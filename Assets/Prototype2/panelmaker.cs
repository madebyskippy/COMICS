using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelmaker : MonoBehaviour {

	[SerializeField] LineRenderer panel;

	LineRenderer currentPanelLine;
	GameObject currentPanelMask;
	LineRenderer lastPanelLine;
	GameObject lastPanelMask;
	Vector3 topleft;



	// Use this for initialization
	void Start () {
		currentPanelLine = null;
		topleft = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene ("prototype2");
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			//delete last panel
			if (currentPanelLine == null) {
				//you're not currently making a panel
				Destroy (lastPanelLine);
				Destroy (lastPanelMask);
				lastPanelLine = null;
				lastPanelMask = null;
			}
		}

		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			topleft = pos;
			currentPanelLine = Instantiate (panel, topleft, Quaternion.identity);
			currentPanelLine.SetPositions (new Vector3[] {topleft,topleft,topleft,topleft});
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Mathf.Abs (currentPanelLine.GetPosition (0).x - currentPanelLine.GetPosition (2).x) < 0.75f ||
				Mathf.Abs (currentPanelLine.GetPosition (0).y - currentPanelLine.GetPosition (2).y) < 0.75f) {
				Debug.Log ("too small");
				Destroy (currentPanelLine);
			} else {
			}
			lastPanelLine = currentPanelLine;
			currentPanelLine = null;
		}

		if (currentPanelLine != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			currentPanelLine.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
			currentPanelLine.SetPosition (2, pos);
			currentPanelLine.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));
		}
	}
}
