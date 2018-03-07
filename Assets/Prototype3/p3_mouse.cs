using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class p3_mouse : MonoBehaviour {

	int currentPanel;

	Vector3 dragStart;
	GameObject currentDrag;

	List<GameObject> row1 = new List<GameObject>();
	List<GameObject> row2 = new List<GameObject>();

	float rowWidth;
	float rowX;

    int row1Count;
    int row2Count;

    public Sprite[] row1sprites;
    public Sprite[] row2sprites;

	// Use this for initialization
	void Start () {
		currentDrag = null;
		dragStart = Vector3.zero;

		row1.Add (GameObject.Find ("panel1"));
		row2.Add (GameObject.Find ("panel2"));

		rowWidth = row1 [0].GetComponent<RectTransform> ().rect.width;
		rowX = row1 [0].GetComponent<RectTransform> ().anchoredPosition.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene ("prototype3");
		}

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
						if (hit.transform.name.Contains("panel1") && currentDrag.name.Contains("p1")) {
							isPlaced = true;
							Destroy (currentDrag);
							addToRow (row1);
                            row1Count++;
                            row1[row1Count].GetComponentsInChildren<Image>()[1].sprite = row1sprites[row1Count];
						} 
						if (hit.transform.name.Contains("panel2") && currentDrag.name.Contains("p2")) {
							isPlaced = true;
							Destroy (currentDrag);
							addToRow (row2);
                            row2Count++;
                            row2[row2Count].GetComponentsInChildren<Image>()[1].sprite = row2sprites[row2Count];
                    
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

	void addToRow(List<GameObject> row){
		GameObject r = Instantiate (row [0].transform.gameObject, row [0].transform.parent);
		row.Add (r);
		float w = rowWidth / row.Count;
		for (int i = 0; i < row.Count; i++) {
			row [i].GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, w*0.95f);
			Vector2 pos = row [i].GetComponent<RectTransform> ().anchoredPosition;
			row [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (rowX+w*i, pos.y);
			Debug.Log (row [i].GetComponent<RectTransform> ().rect.width+","+(rowX+w*i));
		}
	}
}
