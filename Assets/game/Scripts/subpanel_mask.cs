﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subpanel_mask : MonoBehaviour  {
	
	[SerializeField] Image panelImage;
	[SerializeField] BoxCollider2D col;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCollider(){
		Rect rect = GetComponent<RectTransform>().rect;
		float w = rect.width;// * uicanvas.scaleFactor;
		float h = rect.height;// * uicanvas.scaleFactor;
		col.offset = new Vector3 (w / 2, -1 * h / 2, 0);
		col.size = new Vector3 (w, h, 0);
	}

	public void setImage(Sprite s){
		panelImage.sprite = s;
	}
}
