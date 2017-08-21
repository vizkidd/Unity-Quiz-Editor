using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSizeControl : MonoBehaviour {
    public Slider slider;
    public Text text;
    InputField sizeText;
    bool edit;
	// Use this for initialization
	void Start () {
        sizeText = GetComponent<InputField>();
        //sizeText.onValueChanged.AddListener((str) => { EnableEdit(str); });
        sizeText.onEndEdit.AddListener((str)=> { ValueChanged(str); });
	}
	
	// Update is called once per frame
	void Update () {
        if (!edit)
            sizeText.text = text.fontSize.ToString();
	}
    public void EnableEdit()
    {
        edit = true;
    }
    public void ValueChanged(string str)
    {
        Debug.Log(str);
        text.fontSize = Convert.ToInt32(str);
        if (slider.maxValue < text.fontSize)
        {
            slider.maxValue = text.fontSize;
        } 
        edit = false;
    }
}
