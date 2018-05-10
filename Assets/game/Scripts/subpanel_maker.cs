using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subpanel_maker : MonoBehaviour {

	//list of states it should control.
	//e.g.: pg3-row1-s1
	[SerializeField] string[] states;

	[SerializeField] Canvas canvas;
	[SerializeField] RectTransform topleftmarker;
	[SerializeField] LineRenderer line;
	[SerializeField] RectTransform mask;

	[SerializeField] tool_manager toolManager;

	[SerializeField] Image activeBackground;

	Canvas uicanvas;

	LineRenderer currentPanelLine;
	RectTransform currentPanelMask;
	subpanel_collider currentPanelCollider;
	string currentPanelState;
	Vector3 topleft;
	bool isValid;

	Vector2 pagetopleft;

	// Use this for initialization
	void Start () {
		uicanvas = GameObject.Find ("ui").GetComponent<Canvas> ();
		pagetopleft = topleftmarker.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {

		if (toolManager.getTool () == "sub-button") {
			activeBackground.color = new Color (195f/255f, 255f/255f, 168f/255f);
		} else {
			activeBackground.color = Color.white;
		}
		//start panel
		if (Input.GetMouseButtonDown (0)) {
			if (toolManager.getTool () == "sub-button") {
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
				if (hit.collider != null) {
					if (hit.collider.tag == "sub") {
						Debug.Log ("making new panel");
						topleft = Camera.main.ScreenToWorldPoint (Input.mousePosition);
						topleft.z = -2f;
						currentPanelMask = Instantiate (mask, canvas.transform);
						currentPanelMask.position = topleft;
						Vector3 apos = currentPanelMask.anchoredPosition3D;
						apos.z = -2f;
						currentPanelMask.anchoredPosition3D = apos;
						currentPanelMask.GetChild (0).GetComponent<RectTransform> ().anchoredPosition = pagetopleft - currentPanelMask.anchoredPosition;
//					currentPanelLine = Instantiate (line, topleft, Quaternion.identity,canvas.transform);
//					currentPanelLine.SetPositions (new Vector3[] { topleft, topleft, topleft, topleft });
						isValid = true;
						if (hit.collider.name == "subpanel collider") {
							currentPanelState = hit.collider.GetComponent<subpanel_collider> ().getState ();
							currentPanelMask.GetComponent<subpanel_mask> ().setImage (hit.collider.GetComponent<subpanel_collider> ().getImg ());
							currentPanelCollider = hit.collider.GetComponent<subpanel_collider> ();
						} else {
							isValid = false;
						}
					}
				}
			}
		}

		//draw panel
		if (currentPanelMask != null) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag != "sub") {
					isValid = false;
				} else {
					isValid = true;
				}
			} else {
				isValid = false;
			}

			//adjust panel outline
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			pos.z = 0f;
			pos.y = Mathf.Min (pos.y, topleft.y-0.01f);
			pos.x = Mathf.Max (pos.x, topleft.x+0.01f);
//			currentPanelLine.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
//			currentPanelLine.SetPosition (2, pos);
//			currentPanelLine.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));

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
					currentPanelCollider.setColliderState (false);
					globalstate.Instance.setState(currentPanelState,true);
				}
				currentPanelMask = null;
				currentPanelLine = null;
			}
		}
	}
}
