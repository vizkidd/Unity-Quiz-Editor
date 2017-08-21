using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorControl : MonoBehaviour {
    public static ColorPicker picker;
    public Button colorButton;
    public Text textControl;
    //public static bool outline;
    // Use this for initialization
    //public Outline outlineControl;
	void Start () {
        picker = GetComponent<ColorPicker>();

    }
	
	public void ValueChanged()
    {
        if (picker != null)
        {
            textControl.color = picker.CurrentColor;
            ColorBlock block = colorButton.colors;
            block.normalColor = block.highlightedColor = textControl.color;
            colorButton.colors = block;
        }
    }
}
