using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageturner : MonoBehaviour {

	[SerializeField] GameObject[] pages;

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
		}
	}

	public void pageturn(int p){
		allDeactivate ();
		pages [p].SetActive (true);
	}
}
