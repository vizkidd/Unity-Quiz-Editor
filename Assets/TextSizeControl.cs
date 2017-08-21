using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextSizeControl : MonoBehaviour {
    public Text text;
    RectTransform parentRect;
    Slider seeker;
    bool valueChanged;
    public InputField input;
	// Use this for initialization
	void Start () {
        //text = GetComponent<Text>();
        parentRect = GetComponent<RectTransform>();
        seeker = GetComponent<Slider>();
        //seeker.wholeNumbers = true;
        seeker.maxValue = text.cachedTextGenerator.fontSizeUsedForBestFit;
        seeker.minValue = text.resizeTextMinSize;
        
        input.textComponent = text;
    }

    // Update is called once per frame
    void Update() {
        if (!valueChanged)
        {
            //seeker.maxValue = text.resizeTextMaxSize;
            seeker.value = (float)((text.fontSize));
        }
        else
            text.fontSize = (int)seeker.value;
    }

    public void ValueChanged()
    {
        valueChanged = true;
        text.resizeTextForBestFit = false;

        text.fontSize = (int)seeker.value;

    }
    public void StopValueChange()
    {
        valueChanged = false;
        //text.resizeTextMaxSize = (int)seeker.value;

        //text.resizeTextForBestFit = true;

    }

    IEnumerator BestFit(bool value)
    {
        yield return new WaitForEndOfFrame();
        text.resizeTextForBestFit = value;
    }
}
