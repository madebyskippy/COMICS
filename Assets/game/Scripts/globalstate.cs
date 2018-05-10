using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalstate : MonoBehaviour {

	private static globalstate instance = null;

	//========================================================================

	//code help from hang ruan :-)
	public static globalstate Instance {
		get { 
			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
	//========================================================================

	//string goes like this format: pg number - panel number - state number
	Dictionary<string, bool> states;

	// Use this for initialization
	void Start () {
		reload ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reload(){
		states = new Dictionary<string, bool>(){
			{"pg1-row3-s1",true},
			{"pg2-row2-s1",true},
			{"pg2-row3-s1",false},
			{"pg3-row1-s1",false},
			{"pg3-row1-s2",false},
			{"pg3-row1-s3",false},
			{"pg3-row1-s4",false},
			{"pg3-row3-s1",false},
			{"pg3-row3-s2",false},
			{"pg3-row3-s3",false},
			{"pg4-row3-s1",false},
			{"pg4-row3-s2",false}
		};
	}

	public void setState(string s, bool b){
		if (states.ContainsKey (s)) {
			states [s] = b;
		}
	}

	public void addState(string s, bool b){
		if (!states.ContainsKey (s)) {
			states.Add (s, b);
		}
	}

	public bool getState(string s){
		bool b;
		states.TryGetValue (s, out b);
		return b;
	}
}
