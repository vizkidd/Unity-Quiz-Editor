using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour {
    public Canvas[] can;
    bool show=true;

    IEnumerator ShowHideCO()
    {
        yield return new WaitForEndOfFrame();
        foreach (var item in can)
        {
            item.enabled = show;
        }
        if (QuizManager.selectedQuiz != null)
        {
            foreach (var item in QuizManager.selectedQuiz.buttons)
            {
                item.gameObject.SetActive(show);
            }
        }
        yield return null;
    }
    public void ShowOrHide()
    {
        show = !show;
        StartCoroutine(ShowHideCO());
    }
}
