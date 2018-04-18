using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pageturner : MonoBehaviour {

	[SerializeField] GameObject[] pages;
	[SerializeField] Button[] buttons;

	// Use this for initialization
	void Start () {
		allDeactivate ();
		pages [0].SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void allDeactivate(){
		for (int i = 0; i < pages.Length; i++) {
			pages [i].SetActive (false);
			buttons [i].interactable = true;
		}
	}

	public void pageturn(int p){
		allDeactivate ();
		pages [p].SetActive (true);
		buttons [p].interactable = false;
	}
}
