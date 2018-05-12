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

    bool started;


    bool active;
    bool plus;
	// Use this for initialization
	void Start () {
        plus = true;
        started = false;
        enterOnce = false;
        active = false;
        anim = GetComponent<Animator>();
        toolManager = FindObjectOfType<tool_manager>();
	}
	
	// Update is called once per frame
	void Update () {
        started = true;
        if (tag != "pacer")
        {
            if (tag == toolManager.getTool() && name.Substring(name.Length - 1, 1) == toolManager.getActiveRow())
            {
                if (!enterOnce)
                {
                    enterOnce = true;
                    anim.SetBool("enteridle", false);
                    anim.SetBool("enteractive", true);
                }
            }
            else
            {
                enterOnce = false;
                anim.SetBool("enteridle", true);
                anim.SetBool("enteractive", false);
            }
        }
	}

	private void OnEnable()
	{
        if(tag == "pacer" && started && plus){
            Debug.Log(name+" plus");
            anim.SetBool("click", false);
            anim.SetBool("enteridle", true);
            anim.SetBool("enteractive", false);
        } else if (tag=="pacer" && started && !plus){
            Debug.Log(name+" not plus");
            anim.SetBool("click", false);
            anim.SetBool("enteridle", false);
            anim.SetBool("enteractive", true);
        }
	}

	private void OnMouseDown()
	{
        /*if(tag == "pacer"){
            
        }*/
        if (tag == "pacer")
        {
            if (!active)
            {
                plus = false;
                active = true;
                anim.SetBool("click", true);
                anim.SetBool("enteridle", false);
                anim.SetBool("enteractive", true);

            }
            else
            {
                plus = true;
                active = false;

                anim.SetBool("click", true);
                anim.SetBool("enteridle", true);
                anim.SetBool("enteractive", false);
            }
        }

	}

}
