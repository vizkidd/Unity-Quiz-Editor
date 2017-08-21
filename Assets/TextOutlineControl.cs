using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOutlineControl : MonoBehaviour
{
    public InputField x, y;
    public CircleOutline outline;
    Toggle toggler;
    public Button colorButton;
    // Use this for initialization
    void Start()
    {
        toggler = GetComponent<Toggle>();
        y.characterValidation = x.characterValidation = InputField.CharacterValidation.Integer;
        //ValueChanged();
    }
    private void Update()
    {

            x.text = outline.effectDistance.x.ToString();
            y.text = outline.effectDistance.y.ToString();
    }
    public void ValueChanged()
    {
        if(toggler.isOn)
        {
            //y.interactable = x.interactable = true;
            colorButton.interactable = true;
            outline.enabled = true;
            //TextColorControl.outline = true;
        }
        else
        {
            //y.interactable = x.interactable = false;
            colorButton.interactable = false;
            outline.enabled = false;
            //TextColorControl.outline = false;
        }
    }
    public void XValueIncr()
    {
        float tmp = outline.effectDistance.x;
        tmp++;
        outline.effectDistance = new Vector2(tmp, outline.effectDistance.y);
         
    }
    public void XValueDecr()
    {
        float tmp = outline.effectDistance.x;
        tmp--;
        outline.effectDistance = new Vector2(tmp, outline.effectDistance.y);

    }
    public void YValueDecr()
    {
        float tmp = outline.effectDistance.y;
        tmp--;
        outline.effectDistance = new Vector2(outline.effectDistance.x, tmp);

    }
    public void YValueIncr()
    {
        float tmp = outline.effectDistance.y;
        tmp++;
        outline.effectDistance = new Vector2(outline.effectDistance.x, tmp);

    }
    public void ChooseColor()
    {
        outline.effectColor=TextColorControl.picker.CurrentColor;
        ColorBlock block = colorButton.colors;
        block.normalColor = block.highlightedColor = outline.effectColor;
        colorButton.colors = block;
    }
}
