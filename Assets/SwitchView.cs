using UnityEngine;
using UnityEngine.UI;

public class SwitchView : MonoBehaviour {
    public EasyTween view;
    public static bool show = false;
    public void OnClick()
    {
        show = !show;
        if (!show)
        {
            if (QuizManager.selectedQuiz !=null &&QuizManager.selectedQuiz.buttons != null)
            {
                foreach (var item in QuizManager.selectedQuiz.buttons)
                {
                    item.gameObject.SetActive(show);
                }
                foreach (var item in QuizManager.selectedQuiz.textGraphic)
                {
                    item.text.enabled = show;
                }
                QuizManager.selectedQuiz = null;
            }
        }
        view.OpenCloseObjectAnimation();
    }
}
