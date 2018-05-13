using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class profile_manager : MonoBehaviour {

    [SerializeField]
    private GameObject profileText;

    [SerializeField]
    private GameObject ivoryContent, minContent;

    private List<string> existingText;

    [SerializeField]
    private GameObject profileButton;

    /*
     *      {"pg1-row3-s1",true},
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
     * 
     * 
     * */

	// Use this for initialization
	void Start () {
        existingText = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!globalstate.Instance.getState("pg1-row3-s1"))
        {
            AddProfile(minContent, "Secretly likes Ivory barging into his apartment.");
        } else{
            DeleteProfile(minContent, "Secretly likes Ivory barging into his apartment.");  
        }

        if(globalstate.Instance.getState("pg2-row2-s1")){

            if (globalstate.Instance.getState("pg2-row3-s1"))
            {
                AddProfile(minContent, "Sometimes Ivory drives him crazy.");
            }
        } else{
            DeleteProfile(minContent, "Sometimes Ivory drives him crazy.");
        }

        if(globalstate.Instance.getState("pg2-row3-s1")){
            AddProfile(ivoryContent, "Really likes rattling Min.");
        } else{
            DeleteProfile(ivoryContent, "Really likes rattling Min.");
            DeleteProfile(minContent, "Sometimes Ivory drives him crazy.");
        }

        if(globalstate.Instance.getState("pg3-row1-s1") &&
           globalstate.Instance.getState("pg3-row1-s2") &&
           globalstate.Instance.getState("pg3-row1-s3"))
        {
            AddProfile(ivoryContent, "Pretty forgetful.");
            AddProfile(minContent, "Has scary-good memory.");
        } else{

            DeleteProfile(ivoryContent, "Pretty forgetful.");
            DeleteProfile(minContent, "Has scary-good memory.");
        }

        if (globalstate.Instance.getState("pg3-row3-s2"))
        {
            AddProfile(minContent, "Rarely gets mad, but when he does, it's scary.");

            AddProfile(ivoryContent, "Has been learning to yield more.");

        }
        else
        {
            DeleteProfile(minContent, "Rarely gets mad, but when he does, it's scary.");
            DeleteProfile(ivoryContent, "Has been learning to yield more.");
        }

        if(globalstate.Instance.getState("pg4-row3-s2") &&
           globalstate.Instance.getState("pg4-row3-s1")){

            AddProfile(ivoryContent, "Likes it when Min prays so he can stare at his face.");
        } else{
            DeleteProfile(ivoryContent, "Likes it when Min prays so he can stare at his face.");
        
        }

	}

    void AddProfile(GameObject content, string txt){
        if (!existingText.Contains(txt))
        {
            profileButton.GetComponent<Animator>().SetTrigger("pageupdated");
            GameObject info = Instantiate(profileText, content.transform);
            info.GetComponent<Text>().text = txt;
            existingText.Add(txt);
        }
    }

    void DeleteProfile(GameObject content, string txt){
        if(existingText.Contains(txt)){

            profileButton.GetComponent<Animator>().SetTrigger("pagechecked");
            existingText.Remove(txt);
            Text[] txts = content.GetComponentsInChildren<Text>();
            for (int i = 0; i < txts.Length; ++i){
                if(txts[i].text == txt){
                    Destroy(txts[i].gameObject);
                }
            }
        }
    }


}
