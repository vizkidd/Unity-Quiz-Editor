using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAlignmentControl : MonoBehaviour {
    public enum HAlign { Left,Centre,Right };
    public enum VAlign { Top, Middle, Bottom };
    public Button left, centre, right,top,middle,bottom;
    public Text textControl;
    public Color32 normalColor, hightlightColor;
    
    public HAlign hAlign;
    public VAlign vAlign;
    // Use this for initialization
    void Start () {
        hAlign = HAlign.Centre;
        vAlign = VAlign.Middle;
        
	}
	
	// Update is called once per frame
	void Update () {
        switch (hAlign)
        {
            case HAlign.Left:
                switch (vAlign)
                {
                    case VAlign.Top:
                        textControl.alignment = TextAnchor.UpperLeft;
                        break;
                    case VAlign.Middle:
                        textControl.alignment = TextAnchor.MiddleLeft;
                        break;
                    case VAlign.Bottom:
                        textControl.alignment = TextAnchor.LowerLeft;
                        break;
                    default:
                        break;
                }
                break;
            case HAlign.Centre:
                switch (vAlign)
                {
                    case VAlign.Top:
                        textControl.alignment = TextAnchor.UpperCenter;
                        break;
                    case VAlign.Middle:
                        textControl.alignment = TextAnchor.MiddleCenter;
                        break;
                    case VAlign.Bottom:
                        textControl.alignment = TextAnchor.LowerCenter;
                        break;
                    default:
                        break;
                }
                break;
            case HAlign.Right:
                switch (vAlign)
                {
                    case VAlign.Top:
                        textControl.alignment = TextAnchor.UpperRight;
                        break;
                    case VAlign.Middle:
                        textControl.alignment = TextAnchor.MiddleRight;
                        break;
                    case VAlign.Bottom:
                        textControl.alignment = TextAnchor.LowerRight;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        SwitchButton();
    }

    void SwitchButton()
    {
        ColorBlock block;
        switch (hAlign)
        {
            case HAlign.Left:
                block = left.colors;
                block.normalColor = block.highlightedColor = hightlightColor;
                left.colors = block;
                block.normalColor = block.highlightedColor = normalColor;
                centre.colors = right.colors = block;
                break;
            case HAlign.Centre:
                block = centre.colors;
                block.normalColor = block.highlightedColor = hightlightColor;
                centre.colors = block;
                block.normalColor = block.highlightedColor = normalColor;
                left.colors = right.colors = block;
                break;
            case HAlign.Right:
                block = right.colors;
                block.normalColor = block.highlightedColor = hightlightColor;
                right.colors = block;
                block.normalColor = block.highlightedColor = normalColor;
                left.colors = centre.colors = block;
                break;
            default:
                break;
        }
        switch (vAlign)
        {
            case VAlign.Top:
                block = top.colors;
                block.normalColor = block.highlightedColor = hightlightColor;
                top.colors = block;
                block.normalColor = block.highlightedColor = normalColor;
                middle.colors = bottom.colors = block;
                break;
            case VAlign.Middle:
                block = middle.colors;
                block.normalColor = block.highlightedColor= hightlightColor;
                middle.colors = block;
                block.normalColor = block.highlightedColor = normalColor;
                top.colors = bottom.colors = block;
                break;
            case VAlign.Bottom:
                block = bottom.colors;
                block.normalColor = block.highlightedColor = hightlightColor;
                bottom.colors = block;
                block.normalColor = block.highlightedColor= normalColor;
                top.colors = middle.colors = block;
                break;
            default:
                break;
        }
    }
    public void LeftAlign()
    {
        hAlign = HAlign.Left;
    }
    public void CentreAlign()
    {
        hAlign = HAlign.Centre;
    }
    public void RightAlign()
    {
        hAlign = HAlign.Right;
    }
    public void TopAlign()
    {
        vAlign = VAlign.Top;
    }
    public void MiddleAlign()
    {
        vAlign = VAlign.Middle;
    }
    public void BottomAlign()
    {
        vAlign = VAlign.Bottom;
    }
}
