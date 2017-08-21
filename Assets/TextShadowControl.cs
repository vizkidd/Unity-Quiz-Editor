using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShadowControl : MonoBehaviour {
    public Shadow shadow;
    public InputField x, y;
    public Button colorButton;
    Toggle toggler;
	// Use this for initialization
	void Start () {
        toggler = GetComponent<Toggle>();
	}
	
	// Update is called once per frame
	void Update () {
        x.text = shadow.effectDistance.x.ToString();
        y.text = shadow.effectDistance.y.ToString();
    }
    public void XIncr()
    {
        float tmp = shadow.effectDistance.x;
        tmp++;
        shadow.effectDistance = new Vector2(tmp, shadow.effectDistance.y);
    }
    public void YIncr()
    {
        float tmp = shadow.effectDistance.y;
        tmp++;
        shadow.effectDistance = new Vector2(shadow.effectDistance.x,tmp);
    }
    public void XDecr()
    {
        float tmp = shadow.effectDistance.x;
        tmp--;
        shadow.effectDistance = new Vector2(tmp, shadow.effectDistance.y);
    }
    public void YDecr()
    {

        float tmp = shadow.effectDistance.y;
        tmp--;
        shadow.effectDistance = new Vector2(shadow.effectDistance.x, tmp);
    }
    public void ChooseColor()
    {
        shadow.effectColor = TextColorControl.picker.CurrentColor;
        ColorBlock block = colorButton.colors;
        block.normalColor = block.highlightedColor = shadow.effectColor;
        colorButton.colors = block;
    }

    public void ValueChanged()
    {
        if (toggler.isOn)
        {
            // y.interactable = x.interactable = true;
            colorButton.interactable = true;
            shadow.enabled = true;
            //TextColorControl.outline = true;
        }
        else
        {
            //y.interactable = x.interactable = false;
            colorButton.interactable = false;
            shadow.enabled = false;
            //TextColorControl.outline = false;
        }
    }

}
