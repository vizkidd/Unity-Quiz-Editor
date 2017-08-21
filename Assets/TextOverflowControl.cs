using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOverflowControl : MonoBehaviour {
    public Text textControl;
    Dropdown dropDown;
	// Use this for initialization
	void Start () {
        dropDown = GetComponent<Dropdown>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ValueChanged()
    {
        if(dropDown.value ==0)
        {
            textControl.horizontalOverflow = HorizontalWrapMode.Overflow;
        }
        else
        {
            textControl.horizontalOverflow = HorizontalWrapMode.Wrap;
        }
    }
}
