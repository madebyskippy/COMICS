using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subpanel_maker : MonoBehaviour {

	//list of states it should control.
	//e.g.: pg3-p1-s1
	[SerializeField] string[] states;

	[SerializeField] Canvas canvas;
	[SerializeField] RectTransform topleftmarker;
	[SerializeField] LineRenderer line;
	[SerializeField] RectTransform mask;

	Canvas uicanvas;

	LineRenderer currentPanelLine;
	RectTransform currentPanelMask;
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
		//start panel
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				if (hit.collider.tag == "sub") {
					topleft = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					topleft.z = 0f;
					currentPanelMask = Instantiate(mask, canvas.transform);
					currentPanelMask.position = topleft;
					Vector3 apos = currentPanelMask.anchoredPosition3D;
					apos.z = 0f;
					currentPanelMask.anchoredPosition3D = apos;
					currentPanelMask.GetChild (0).GetComponent<RectTransform> ().anchoredPosition = pagetopleft - currentPanelMask.anchoredPosition;
					currentPanelLine = Instantiate (line, topleft, Quaternion.identity,canvas.transform);
					currentPanelLine.SetPositions (new Vector3[] { topleft, topleft, topleft, topleft });
					isValid = true;
					if (hit.collider.name == "subpanel collider") {
						currentPanelState = hit.collider.GetComponent<subpanel_collider> ().getState();
						currentPanelMask.GetComponent<subpanel_mask> ().setImage(hit.collider.GetComponent<subpanel_collider> ().getImg ());
					}
				}
			}
		}

		//draw panel
		if (currentPanelLine != null) {
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
			currentPanelLine.SetPosition (1, new Vector3 (topleft.x, pos.y, 0f));
			currentPanelLine.SetPosition (2, pos);
			currentPanelLine.SetPosition (3, new Vector3 (pos.x, topleft.y, 0f));

			//adjust panel mask/filling
			Vector3 screenpos = Camera.main.WorldToScreenPoint (pos) / uicanvas.scaleFactor;
			float width = screenpos.x - Camera.main.WorldToScreenPoint (topleft).x / uicanvas.scaleFactor;
			float height = Camera.main.WorldToScreenPoint (topleft).y /uicanvas.scaleFactor - screenpos.y;
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, width);
			currentPanelMask.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, height);
		}

		//end panel
		if (Input.GetMouseButtonUp (0)) {
			if (currentPanelLine != null) {
				if (!isValid) {
					Destroy (currentPanelLine.gameObject);
					Destroy (currentPanelMask.gameObject);
				} else {
					//panel was created!! woo hoo
					currentPanelMask.GetComponent<subpanel_mask>().setCollider();
					globalstate.Instance.setState(currentPanelState,true);
				}
				currentPanelMask = null;
				currentPanelLine = null;
			}
		}
	}
}
