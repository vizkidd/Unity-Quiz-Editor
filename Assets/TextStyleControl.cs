using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextStyleControl : MonoBehaviour {
    public Text textControl;
    Dropdown dropDown;
    // Use this for initialization
    void Start()
    {
        dropDown = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ValueChanged()
    {

        switch (dropDown.value)
        {
            case 0:
                textControl.fontStyle = FontStyle.Normal;
                break;
            case 1:
                textControl.fontStyle = FontStyle.Bold;
                break;
            case 2:
                textControl.fontStyle = FontStyle.Italic;
                break;
            case 3:
                textControl.fontStyle = FontStyle.BoldAndItalic;
                break;
            default:
                break;
        }
     
    }
}
