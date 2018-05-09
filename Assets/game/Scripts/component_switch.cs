using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class component_switch : MonoBehaviour {

	//just switches anything on when a state is met

	[SerializeField] GameObject component;

	[Header("state to trigger on/off with the component")]
	[SerializeField] string stateTrigger;

	[Space(20)]
	[Header("true states")]
	[SerializeField] bool hasRangeTrue; //true if it only needs some of the list to be true
	[SerializeField] Vector2 rangeTrue; //the min/max # that have to be true
	[SerializeField] string[] trueStates; //states that need to be true for this to show

	[Header("false states")]
	[SerializeField] bool hasRangeFalse;
	[SerializeField] Vector2 rangeFalse;
	[SerializeField] string[] falseStates; //states that need to be false for this to show

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool s = checkState ();
		component.SetActive (s);
		if (stateTrigger != "" && globalstate.Instance.getState(stateTrigger)!=s)
			globalstate.Instance.setState (stateTrigger,s);
	}

	bool checkState(){
		bool state = true;
		int numTrue = 0;
		for (int i = 0; i < trueStates.Length; i++) {
			if (!globalstate.Instance.getState (trueStates [i])) {
				state = false;
			} else {
				numTrue++;
			}
		}

		int numFalse = 0;
		for (int i = 0; i < falseStates.Length; i++) {
			if (globalstate.Instance.getState (falseStates [i])) {
				state = false;
			} else {
				numFalse++;
			}
		}

		if (hasRangeTrue) {
			if (numTrue >= rangeTrue.x && numTrue <= rangeTrue.y) {
				state = true;
			} else {
				state = false;
			}
		}
		if (hasRangeFalse) {
			if (numFalse >= rangeFalse.x && numFalse <= rangeFalse.y) {
				state = true;
			} else {
				state = false;
			}
		}

		return state;
	}
}
