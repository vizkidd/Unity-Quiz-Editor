using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonBehaviour : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public static Color32 highlightColor;
    Button button;
    Color32 lowlight;
    private void Start()
    {
        button = GetComponent<Button>();
        lowlight = button.colors.highlightedColor;
        highlightColor = new Color32(160, 255, 69, 255);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        var colors = button.colors;
        colors.highlightedColor = highlightColor;
        button.colors = colors;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var colors = button.colors;
        colors.highlightedColor = lowlight;
        button.colors = colors;
    }
}
