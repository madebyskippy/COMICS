using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

	bool isValid;

	List<RectTransform> masks;

	// Use this for initialization
	void Start () {
		currentPanelLine = null;
		currentPanelMask = null;
		topleft = Vector3.zero;
		isValid = false;
		masks = new List<RectTransform> ();
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
				Destroy (lastPanelLine.gameObject);
				Destroy (lastPanelMask.gameObject);
				lastPanelLine = null;
				lastPanelMask = null;
				masks.RemoveAt (masks.Count-1);
			}
		}

		//start drawing panel
		if (Input.GetMouseButtonDown (0)) {
			if (masks.Count < 4) {
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
				isValid = false;
			}
		}

		//finish drawing the panel
		if (Input.GetMouseButtonUp (0)) {
			if (currentPanelLine != null) {
				if (!isValid) {
					Debug.Log ("it's red.");
					Destroy (currentPanelLine.gameObject);
					Destroy (currentPanelMask.gameObject);
				} else {
					//panel was created!! woo hoo
					lastPanelLine = currentPanelLine;
					lastPanelMask = currentPanelMask;

					float r1x = currentPanelMask.anchoredPosition.x;
					float r1y = currentPanelMask.anchoredPosition.y;
					float r1w = (currentPanelMask.offsetMax - currentPanelMask.anchoredPosition).x;
					float r1h = -1*(currentPanelMask.offsetMin - currentPanelMask.anchoredPosition).y;
					Debug.Log (r1x+","+r1y+", "+r1w+","+r1h);
					masks.Add (currentPanelMask);
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
			//honestly idk if i need screen point anymore since the canvas is camera space overlay but it works
			//so i'm keeping it
			Vector3 screenpos = Camera.main.WorldToScreenPoint(pos) / canvas.scaleFactor;
			float width = screenpos.x - Camera.main.WorldToScreenPoint (topleft).x / canvas.scaleFactor;
			float height = Camera.main.WorldToScreenPoint (topleft).y / canvas.scaleFactor - screenpos.y;
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);

			if (Mathf.Abs (currentPanelLine.GetPosition (0).x - currentPanelLine.GetPosition (2).x) < 0.75f ||
			    Mathf.Abs (currentPanelLine.GetPosition (0).y - currentPanelLine.GetPosition (2).y) < 0.75f) {
				currentPanelLine.material = red;
				isValid = false;
			}else if (	Mathf.Abs (currentPanelLine.GetPosition (0).x - currentPanelLine.GetPosition (2).x) > 5f ||
				Mathf.Abs (currentPanelLine.GetPosition (0).y - currentPanelLine.GetPosition (2).y) > 5f){
				currentPanelLine.material = red;
				isValid = false;
			}else{
				currentPanelLine.material = black;
				isValid = true;
			}

			for (int i = 0; i < masks.Count; i++) {
				if (isRectOverlap (currentPanelMask, masks [i])) {
					//they're overlapping )-:
					currentPanelLine.material = red;
					isValid = false;
					break;
				}
			}
		}
	}

	//from http://jeffreythompson.org/collision-detection/rect-rect.php
	//thank u god
	bool isRectOverlap(RectTransform r1, RectTransform r2){
		float r1x = r1.anchoredPosition.x;
		float r1y = r1.anchoredPosition.y;
		float r1w = (r1.offsetMax - r1.anchoredPosition).x;
		float r1h = -1*(r1.offsetMin - r1.anchoredPosition).y;
		float r2x = r2.anchoredPosition.x;
		float r2y = r2.anchoredPosition.y;
		float r2w = (r2.offsetMax - r2.anchoredPosition).x;
		float r2h = -1*(r2.offsetMin - r2.anchoredPosition).y;

		if (r1x + r1w >= r2x &&     // r1 right edge past r2 left
			r1x <= r2x + r2w &&       // r1 left edge past r2 right
			r1y - r1h <= r2y &&       // btm r1  < r2 top
			r1y >= r2y - r2h) {       // r1 top > r2 btm
			return true;
		}
		return false;
	}
}
