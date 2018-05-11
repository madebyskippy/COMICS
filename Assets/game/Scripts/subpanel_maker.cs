using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subpanel_maker : MonoBehaviour {

	//list of states it should control.
	//e.g.: pg3-row1-s1
	[SerializeField] string[] states;
	[SerializeField] Sprite[] panelImages;

	[SerializeField] Canvas canvas;
	[SerializeField] GameObject subpanelParent;
	[SerializeField] RectTransform topleftmarker;
	[SerializeField] LineRenderer line;
	[SerializeField] RectTransform mask;

	[SerializeField] tool_manager toolManager;

	[SerializeField] Image activeBackground;

	[SerializeField] Text countdown;

	Canvas uicanvas;

	LineRenderer currentPanelLine;
	RectTransform currentPanelMask;
	subpanel_collider currentPanelCollider;
	string currentPanelState;
	Vector3 topleft;
	bool isValid;

	Vector2 pagetopleft;

	Vector2 maxSizes = new Vector2(2f,2f);
	int maxNumOfPanels = 3;
	int numPanels = 0;
	int nextPanel = 0;

	// Use this for initialization
	void Start () {
		uicanvas = GameObject.Find ("ui").GetComponent<Canvas> ();
		pagetopleft = topleftmarker.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {

		countdown.text = (maxNumOfPanels - numPanels).ToString();
		updateNumPanels ();

		if (toolManager.getTool () == "sub-button") {
			activeBackground.color = new Color (195f/255f, 255f/255f, 168f/255f);
		} else {
			activeBackground.color = Color.white;
		}
		//start panel
		if (Input.GetMouseButtonDown (0)) {
			if (toolManager.getTool () == "sub-button" && numPanels < maxNumOfPanels) {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					if (hit.collider.tag == "sub") {
						topleft = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						topleft.z = -2f;
						currentPanelMask = Instantiate (mask, subpanelParent.transform);
						currentPanelMask.position = topleft;
						Vector3 apos = currentPanelMask.anchoredPosition3D;
						apos.z = -2f;
						currentPanelMask.anchoredPosition3D = apos;
						isValid = true;
//						if (hit.collider.name == "subpanel collider") {
//							currentPanelState = hit.collider.GetComponent<subpanel_collider> ().getState ();
//							currentPanelMask.GetComponent<subpanel_mask> ().setImage (hit.collider.GetComponent<subpanel_collider> ().getImg ());
//							currentPanelCollider = hit.collider.GetComponent<subpanel_collider> ();
//						} else {
//							isValid = false;
//						}
						currentPanelState = states[nextPanel];
						currentPanelMask.GetComponent<subpanel_mask> ().setImage (panelImages [nextPanel]);
					}
				}
			}
		}

		//draw panel
		if (currentPanelMask != null) {

			//adjust panel outline
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			pos.y = Mathf.Min (pos.y, topleft.y-0.01f);
			pos.x = Mathf.Max (pos.x, topleft.x+0.01f);

			pos.y = Mathf.Max (pos.y, topleft.y - maxSizes.y);
			pos.x = Mathf.Min (pos.x, topleft.x + maxSizes.x);
//			currentPanelLine.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
//			currentPanelLine.SetPosition (2, pos);
//			currentPanelLine.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));

			RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag != "sub") {
					isValid = false;
				} else {
					isValid = true;
				}
			} else {
				isValid = false;
			}

			//adjust panel mask/filling
			Vector3 screenpos = Camera.main.WorldToScreenPoint (pos) / uicanvas.scaleFactor;
			float width = screenpos.x - Camera.main.WorldToScreenPoint (topleft).x / uicanvas.scaleFactor;
			float height = Camera.main.WorldToScreenPoint (topleft).y /uicanvas.scaleFactor - screenpos.y;
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);

			if (width < 50 || height < 50) {
				isValid = false;
			}
		}

		//end panel
		if (Input.GetMouseButtonUp (0)) {
			if (currentPanelMask != null) {
				if (!isValid) {
					Destroy (currentPanelMask.gameObject);
				} else {
					//panel was created!! woo hoo
					currentPanelMask.GetComponent<subpanel_mask>().setCollider();
					currentPanelMask.GetComponent<subpanel_mask> ().setState (currentPanelState);
					currentPanelMask.GetComponent<subpanel_mask> ().setAreaCollider (currentPanelCollider);
//					currentPanelCollider.setColliderState (false);
					globalstate.Instance.setState(currentPanelState,true);
				}
				currentPanelMask = null;
				currentPanelLine = null;
			}
		}
	}

	void updateNumPanels(){
		numPanels = 0;
		nextPanel = 0;
		for (int i = maxNumOfPanels-1; i >0; i--) {
			if (globalstate.Instance.getState ("pg3-row1-s" + (i + 1).ToString ())) {
				numPanels++;
				Debug.Log ("pg3-row1-s" + (i + 1).ToString () + "state is true " + numPanels);
			} else {
				nextPanel = i;
			}
		}
	}
}
