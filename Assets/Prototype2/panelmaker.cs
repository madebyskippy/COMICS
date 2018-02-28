using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class panelmaker : MonoBehaviour {

	[SerializeField] Canvas canvas;
	[SerializeField] LineRenderer panel;
	[SerializeField] RectTransform mask;
	[SerializeField] Material red;
	[SerializeField] Material black;

	[SerializeField] Sprite[] group1;
	[SerializeField] Sprite[] group2;
	[SerializeField] Sprite[] group3;
	[SerializeField] Sprite[] group4;
	[SerializeField] Sprite[] group5;

	bool[][] panelstate = new bool[5][];

	Dictionary<Group, Sprite[]> grouppanels;

	[SerializeField] panelswitcher[] switcher;

	[SerializeField] Vector2[] maxSizes;

	Group currentGroup;
	LineRenderer currentPanelLine;
	RectTransform currentPanelMask;
	LineRenderer lastPanelLine;
	RectTransform lastPanelMask;
	Vector3 topleft;

	bool isValid;

	List<RectTransform> masks;
	List<LineRenderer> lines;

	Image[] panelContent = new Image[5];

	bool hasPhone;

	// Use this for initialization
	void Start () {
		currentPanelLine = null;
		currentPanelMask = null;
		topleft = Vector3.zero;
		isValid = false;
		masks = new List<RectTransform> ();
		lines = new List<LineRenderer> ();

		grouppanels = new Dictionary<Group,Sprite[]> ();
		grouppanels.Add (Group.bed, group1);
		grouppanels.Add (Group.clothes, group2);
		grouppanels.Add (Group.piano, group3);
		grouppanels.Add (Group.chair, group4);
		grouppanels.Add (Group.door, group5);

		panelstate [(int)Group.bed] = new bool[2];
		panelstate [(int)Group.bed] [0] = false; // not out of bed
		panelstate [(int)Group.bed] [1] = false; // no phone
		panelstate [(int)Group.clothes] = new bool[1]; 
		panelstate [(int)Group.clothes] [0] = false; // not wearing clothes
		panelstate [(int)Group.piano] = new bool[1]; 
		panelstate [(int)Group.piano] [0] = false; // not listening to music
		panelstate [(int)Group.chair] = new bool[1]; 
		panelstate [(int)Group.chair] [0] = false; // not rushed for time
		panelstate [(int)Group.door] = new bool[1]; 
		panelstate [(int)Group.door] [0] = false; // not rushed for time

		hasPhone = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){
			SceneManager.LoadScene ("Prototype2/protoype2");
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			//delete last panel
			if (currentPanelLine == null) {
				if (masks.Count > 0) {
					RectTransform temp = masks [masks.Count - 1];
					masks.RemoveAt (masks.Count - 1);
					Destroy (temp.gameObject);
					LineRenderer tempLine = lines [lines.Count - 1];
					lines.RemoveAt (lines.Count - 1);
					Destroy (tempLine.gameObject);
				}
			}
		}

		//start drawing panel
		if (Input.GetMouseButtonDown (0)) {
			if (masks.Count < 5) {
				bool isInPanel = false;
				int g = -1;	
				for (int i = 0; i < 5; i++) {
					if (switcher [i].isIn) {
						isInPanel= true;
						g = i;
						break;
					}
				}
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				pos.z = 0f;
				topleft = pos;
				currentPanelMask = Instantiate(mask, canvas.transform);
				currentPanelMask.position = pos;
				if (g != -1) {
					Sprite s = null;
					currentGroup = (Group)g;
					s = getSprite (g);
					if (s != null) {
						panelContent [g] = currentPanelMask.GetChild (0).GetComponent<Image> ();
						panelContent [g].sprite = s;
					}
				}
				currentPanelMask.GetChild (0).GetComponent<RectTransform> ().anchoredPosition = -1 * currentPanelMask.anchoredPosition;
				currentPanelLine = Instantiate (panel, topleft, Quaternion.identity);
				currentPanelLine.SetPositions (new Vector3[] { topleft, topleft, topleft, topleft });
				isValid = false;
			}
		}

		//finish drawing the panel
		if (Input.GetMouseButtonUp (0)) {
			if (currentPanelLine != null) {
				if (!isValid) {
					Destroy (currentPanelLine.gameObject);
					Destroy (currentPanelMask.gameObject);
				} else {
					//panel was created!! woo hoo
					lastPanelLine = currentPanelLine;
					lastPanelMask = currentPanelMask;
					masks.Add (currentPanelMask);
					lines.Add (currentPanelLine);
				}
				currentPanelMask = null;
				currentPanelLine = null;
			}
		}

		for (int i = 0; i < 5; i++) {
			//check & refresh state
			if (panelContent [i] != null) {
				Sprite s = getSprite (i);
				panelContent [i].sprite = s;
				panelstate [i][0] = true;
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


			if (currentGroup == Group.bed) {
				if (switcher [5].isIn) {
					//u touched the phone
					panelstate [(int)Group.bed] [1] = true;
					currentPanelMask.GetChild (0).GetComponent<Image> ().sprite = grouppanels [Group.bed] [1];
					hasPhone = true;
				} else {
					panelstate [(int)Group.bed] [1] = false;
					currentPanelMask.GetChild (0).GetComponent<Image> ().sprite = grouppanels [Group.bed] [0];
					hasPhone = false;
				}
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

	Sprite getSprite(int g){
		Sprite s = null;
		switch (g) {
		case 0:
			//if you hit the phone or not
			//ur awake
			s = grouppanels [Group.bed] [0];
			panelstate [(int)Group.bed] [0] = true; //ur awake
			if (hasPhone) {
				s = grouppanels [Group.bed] [1];
			}
			panelstate [(int)Group.bed] [1] = hasPhone;
			break;
		case 1:
			//if you're awake
			if (panelstate [(int)Group.bed] [0]) {
				if (panelstate [(int)Group.bed] [1]) {
					//have phone
					s = grouppanels[Group.clothes][1];
				} else {
					//no phone
					s = grouppanels[Group.clothes][0];
				}
				panelstate [(int)Group.clothes] [0] = true;
			}
			break;
		case 2:
			if (panelstate [(int)Group.bed] [0]) {
				if (panelstate [(int)Group.bed] [1] && panelstate[(int)Group.clothes][0]) {
					//have phone
					s = grouppanels[Group.piano][3];
				}
				if (panelstate [(int)Group.bed] [1] && !panelstate[(int)Group.clothes][0]) {
					//have phone
					s = grouppanels[Group.piano][2];
				} 
				if (!panelstate [(int)Group.bed] [1] && panelstate[(int)Group.clothes][0]) {
					//have phone
					s = grouppanels[Group.piano][1];
				} 
				if (!panelstate [(int)Group.bed] [1] && !panelstate[(int)Group.clothes][0]) {
					//have phone
					s = grouppanels[Group.piano][0];
				} 
				panelstate [(int)Group.piano] [0] = true;
			}
			break;
		case 3:
			if (panelstate [(int)Group.bed] [0]) {
				if (panelstate [(int)Group.bed] [1]) {
					//have phone
					if (panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.piano][0]) {
						s = grouppanels [Group.chair] [2];
					}
					if (panelstate [(int)Group.clothes] [0] && !panelstate[(int)Group.piano][0]) {
						s = grouppanels [Group.chair] [1];
					}
					if (!panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.piano][0]) {
						s = grouppanels [Group.chair] [3];
					}
					if (!panelstate [(int)Group.clothes] [0] && !panelstate[(int)Group.piano][0]) {
						s = grouppanels [Group.chair] [0];
					}
					panelstate [(int)Group.chair] [0] = true;
				} else {
					//no phone
					s = null;
				}
			}
			break;
		case 4:
			if (panelstate [(int)Group.bed] [0]) {
				if (!panelstate [(int)Group.clothes] [0] && !panelstate[(int)Group.bed][1] && !panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [0];
				}
				if (panelstate [(int)Group.clothes] [0] && !panelstate[(int)Group.bed][1] && !panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [1];
				}
				if (!panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.bed][1] && !panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [2];
				}
				if (!panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.bed][1] && panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [3];
				}
				if (panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.bed][1] && !panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [4];
				}
				if (panelstate [(int)Group.clothes] [0] && panelstate[(int)Group.bed][1] && panelstate[(int)Group.chair][0]) {
					s = grouppanels [Group.door] [5];
				}
			}
			break;
		}
		return s;
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
