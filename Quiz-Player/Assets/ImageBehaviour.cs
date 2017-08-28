using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageBehaviour : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    public Color32 highlightColor;
    Image button;
    Color32 lowlight;
    private void Start()
    {
        button = GetComponent<Image>();
        lowlight = button.color;
        highlightColor = new Color32(160, 255, 69, 255);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        var color = button.color;
        color = highlightColor;
        button.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var color = button.color;
        color = lowlight;
        button.color = color;
    }
}
