using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p3_mouse : MonoBehaviour {

	int currentPanel;

	Vector3 dragStart;
	GameObject currentDrag;

	// Use this for initialization
	void Start () {
		currentDrag = null;
		dragStart = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Input.mousePosition, Vector2.zero);
			if (hit.collider != null) {
				Debug.Log (hit.transform.tag);
				if (hit.transform.tag == "draggable") {
					currentDrag = hit.transform.gameObject;
					dragStart = currentDrag.transform.position;
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if (currentDrag != null) {
				RaycastHit2D hit = Physics2D.Raycast (Input.mousePosition, Vector2.zero, Mathf.Infinity, 5);
				bool isPlaced = false;
				if (hit.collider != null) {
					Debug.Log (hit.transform.tag);
					if (hit.transform.tag == "target") {
						if (hit.transform.name == "panel1" && currentDrag.name == "p1") {
							isPlaced = true;
						} 
						if (hit.transform.name == "panel2" && currentDrag.name == "p2") {
							isPlaced = true;
						}
					}
				}
				if (!isPlaced) {
					//wasn't added to the page, go back to where u came from
					currentDrag.transform.position = dragStart;
				}
				currentDrag = null;
			}
		}

		if (currentDrag != null) {
			currentDrag.transform.position = Input.mousePosition;
		}
	}
}
