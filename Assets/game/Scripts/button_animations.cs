using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class button_animations : MonoBehaviour {

    /*
     * sub-button - animates while active
     * pacer - click it and animates once
     * gutter-button - animates while active
     * */

    Animator anim;
    tool_manager toolManager;
    bool enterOnce;
	// Use this for initialization
	void Start () {
        enterOnce = false;
        anim = GetComponent<Animator>();
        toolManager = FindObjectOfType<tool_manager>();
	}
	
	// Update is called once per frame
	void Update () {
       /* switch(toolManager.getTool()){
            case "null":
                enterOnce = false;
                anim.SetBool("enteridle", true);
                anim.SetBool("enteractive", false);
                break;
            case "sub-button":
                SubButtonActive();
                break;
            case "gutter-button":
                GutterButtonActive();
                break;
        }*/
        if(tag == toolManager.getTool()){
            if (!enterOnce)
            {
                enterOnce = true;
                anim.SetBool("enteridle", false);
                anim.SetBool("enteractive", true);
            }
        } else{
            enterOnce = false;
            anim.SetBool("enteridle", true);
            anim.SetBool("enteractive", false);
        }
	}

    void SubButtonActive(){
        if (!enterOnce)
        {
            enterOnce = true;
            anim.SetBool("enteridle", false);
            anim.SetBool("enteractive", true);
        }
    }
    void GutterButtonActive(){
        if(!enterOnce){
            enterOnce = true;
            anim.SetBool("enteridle", false);
            anim.SetBool("enteractive", true);
        }
    }

}
