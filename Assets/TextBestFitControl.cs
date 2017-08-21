using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBestFitControl : MonoBehaviour {
    public Text textControl;
    public Slider fontSizeControl;
    Toggle toggler;
    // Use this for initialization
	void Start () {
        toggler = GetComponent<Toggle>();
        ValueChanged();
	}

   

    public void ValueChanged()
    {
        if(toggler.isOn)
        {
            textControl.resizeTextForBestFit = true;
            fontSizeControl.interactable = false;
        }
        else
        {
            textControl.resizeTextForBestFit = false;
            fontSizeControl.interactable = true;
        }
    }
}
