using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pageturner : MonoBehaviour {

    [SerializeField] GameObject coverpage;
    [SerializeField] GameObject titleSidebar;

	[SerializeField] GameObject[] pages;
	[SerializeField] Button[] pageButtons;

	[SerializeField] Image[] buttonsOnPage1;
	[SerializeField] Image[] buttonsOnPage2;
	[SerializeField] Image[] buttonsOnPage3;
	[SerializeField] Image[] buttonsOnPage4;

	[SerializeField] GameObject mouse;
	[SerializeField] GameObject pageLeft;
	[SerializeField] GameObject pageRight;

	int activePage;
	List<Image[]> buttonsOnPages = new List<Image[]> ();
	bool mouseOnPage;

	// Use this for initialization
	void Start () {
        coverpage.SetActive(true);
        titleSidebar.SetActive(false);
		for (int i = 0; i < 4; i++) {
			pages [i].SetActive (true);
		}
		allDeactivate ();
		pages[0].GetComponent<RectTransform>().position = new Vector3(0,0,91);
		//pageButtons[0].interactable = false;
        activePage = 0;
		buttonsOnPages.Add (buttonsOnPage1);
		buttonsOnPages.Add (buttonsOnPage2);
		buttonsOnPages.Add (buttonsOnPage3);
		buttonsOnPages.Add (buttonsOnPage4);
		hideAllButtons ();
		mouseOnPage = false;
	}
	
	// Update is called once per frame
	void Update () {
		bool mop;
		if (mouse.transform.position.x > pageLeft.transform.position.x &&
		    mouse.transform.position.x < pageRight.transform.position.x) {
			mop = true;
		} else {
			mop = false;
		}
		if (mouseOnPage != mop) {
			if (mop) {
				showButtons ();
			} else {
				hideAllButtons ();
			}
		}
		mouseOnPage = mop;
	}

	void allDeactivate(){
		for (int i = 0; i < pages.Length; i++) {
//			pages [i].SetActive (false);
			pages[i].GetComponent<RectTransform>().position = new Vector3(0,20,91);
			pageButtons [i].interactable = true;
		}
	}

	public void pageturn(int p){
        coverpage.SetActive(false);

        titleSidebar.SetActive(true);

		allDeactivate ();
//		pages [p].SetActive (true);
		pages[p].GetComponent<RectTransform>().position = new Vector3(0,0,91);
		pageButtons [p].interactable = false;
		activePage = p;
		pageButtons [p].GetComponent<Animator> ().SetTrigger ("pagechecked");
	}

	void showButtons(){
		Image[] active = buttonsOnPages [activePage];
		for (int i = 0; i < active.Length; i++) {
			active [i].color = new Color (1, 1, 1, 1);
		}
	}

	void hideAllButtons(){
		for (int j = 0; j < buttonsOnPages.Count; j++) {
			Image[] active = buttonsOnPages [j];
			for (int i = 0; i < active.Length; i++) {
				active [i].color = new Color (1, 1, 1, 0);
			}
		}
	}

	public void markUnread(int page){
		if (activePage != page) {
			pageButtons [page].GetComponent<Animator> ().SetTrigger ("pageupdated");
		}
	}
}
