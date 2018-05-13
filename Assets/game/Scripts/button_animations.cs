using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
    bool enter;
	// Use this for initialization
	void Start () {
        enter = false;
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

        if(GetComponent<Image>().color.a > 0 && !enter){
            enter = true;
            AnimateEnter();
        } else if(GetComponent<Image>().color.a < 1 && enter){
            enter = false;
        }
	}

	private void OnEnable()
	{
       // Debug.Log("what");

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

    private void AnimateEnter(){
        //GetComponent<RectTransform>().DOScale(2f, 0.5f);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetComponent<RectTransform>().DOScale(2f, 0.5f));
        sequence.Append(GetComponent<RectTransform>().transform.DOScale(0.8f, 0.5f));
        sequence.Append(GetComponent<RectTransform>().transform.DOScale(1f, 0.2f));
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
