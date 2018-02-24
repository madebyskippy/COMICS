using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panelmaker : MonoBehaviour {

	[SerializeField] Canvas canvas;
	[SerializeField] LineRenderer panel;
	[SerializeField] RectTransform mask;
	[SerializeField] Material red;
	[SerializeField] Material black;

	[SerializeField] Sprite[] group1;
	[SerializeField] Sprite[] group2;

	LineRenderer currentPanelLine;
	RectTransform currentPanelMask;
	LineRenderer lastPanelLine;
	RectTransform lastPanelMask;
	Vector3 topleft;

	bool isValidSize;
	int numPanels;

	// Use this for initialization
	void Start () {
		currentPanelLine = null;
		currentPanelMask = null;
		topleft = Vector3.zero;
		numPanels = 0;
		isValidSize = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene ("Prototype2/protoype2");
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

		//start drawing panel
		if (Input.GetMouseButtonDown (0)) {
			if (numPanels < 4) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				pos.z = 0f;
				topleft = pos;
				/*
				 *put stuff here to determine which group & panel to unmask
				 *for group: check pos.x and pos.y within certain range
				 *for panel: check some booleans or something like that :-P
				 *temporary for now i'm just doing the first one in group 1
				 */
				currentPanelMask = Instantiate(mask, canvas.transform);
				currentPanelMask.position = pos;//new Vector3 (Input.mousePosition.x,Input.mousePosition.y, 0f);
				currentPanelLine = Instantiate (panel, topleft, Quaternion.identity);
				currentPanelLine.SetPositions (new Vector3[] { topleft, topleft, topleft, topleft });
				isValidSize = false;
			}
		}

		//finish drawing the panel
		if (Input.GetMouseButtonUp (0)) {
			if (currentPanelLine != null) {
				if (!isValidSize) {
					Debug.Log ("it's red.");
					Destroy (currentPanelLine.gameObject);
				} else {
					//panel was created!! woo hoo
					lastPanelLine = currentPanelLine;
					lastPanelMask = currentPanelMask;
					numPanels++;
				}
				currentPanelMask = null;
				currentPanelLine = null;
			}
		}

		//draw the panel
		if (currentPanelLine != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			pos.y = Mathf.Min (pos.y, topleft.y-0.01f);
			pos.x = Mathf.Max (pos.x, topleft.x+0.01f);
			currentPanelLine.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
			currentPanelLine.SetPosition (2, pos);
			currentPanelLine.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));

			//this is dumb lol... gotta love prototypes
			Vector3 screenpos = Camera.main.WorldToScreenPoint(pos) / canvas.scaleFactor;
			float width = screenpos.x - Camera.main.WorldToScreenPoint (topleft).x / canvas.scaleFactor;
			float height = Camera.main.WorldToScreenPoint (topleft).y / canvas.scaleFactor - screenpos.y;
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);

			if (Mathf.Abs (currentPanelLine.GetPosition (0).x - currentPanelLine.GetPosition (2).x) < 0.75f ||
			    Mathf.Abs (currentPanelLine.GetPosition (0).y - currentPanelLine.GetPosition (2).y) < 0.75f) {
				currentPanelLine.material = red;
				isValidSize = false;
			}else if (	Mathf.Abs (currentPanelLine.GetPosition (0).x - currentPanelLine.GetPosition (2).x) > 5f ||
				Mathf.Abs (currentPanelLine.GetPosition (0).y - currentPanelLine.GetPosition (2).y) > 5f){
				currentPanelLine.material = red;
				isValidSize = false;
			}else{
				currentPanelLine.material = black;
				isValidSize = true;
			}
		}
	}
}
