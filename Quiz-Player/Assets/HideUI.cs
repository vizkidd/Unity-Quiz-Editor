using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour {
    public static bool hidden = true;
    public EasyTween hideSeeker;
    public void Show()
    {
        if(hidden)
        {
            StartCoroutine(ShowHideCO());
        }
    }
    public void Hide()
    {
        if(!hidden)
        {
            StartCoroutine(ShowHideCO());
        }
    }
    IEnumerator ShowHideCO()
    {
        yield return new WaitForEndOfFrame();
        hidden = !hidden;
        hideSeeker.OpenCloseObjectAnimation();
        
    }
}
