using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tool_manager : MonoBehaviour {

	string currentTool;
	Image img;
	string activeRow;

	// Use this for initialization
	void Start () {
		currentTool = "null";
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = Input.mousePosition;
		pos.z = 10f;
		transform.position = Camera.main.ScreenToWorldPoint(pos);

		if (Input.GetMouseButtonDown(0)){
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			string newTool;
			if (hit.collider != null) {
				newTool = hit.collider.tag;
			} else {
				newTool = "null";
				Debug.Log ("null");
			}

			if (newTool == "sub" && currentTool == "sub-button") {
				newTool = "sub-button";
			}
			if (newTool == "gutter" && currentTool == "gutter-button") {
				newTool = "gutter-button";
			}

			currentTool = newTool;

			if (currentTool == "sub-button" || currentTool == "gutter-button" || currentTool == "pacer"){
				activeRow = hit.collider.name.Substring (hit.collider.name.Length - 1, 1);
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			globalstate.Instance.reload ();
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public string getTool(){
		return currentTool;
	}

	public string getActiveRow(){
		return activeRow;
	}
}
