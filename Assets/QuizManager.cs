using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class QuizManager : MonoBehaviour {
    public static List<Quiz> quizList;
    public static Quiz selectedQuiz;
    Menu menu;
    static VideoPlayer player;

    public EasyTween selectQuizTween, createQuizTween;

    void Start () {
        player = GameObject.FindObjectOfType<VideoPlayer>();
        quizList = new List<Quiz>();
        menu = GameObject.FindObjectOfType<Menu>();
	}
	
	// Update is called once per frame
	public static void SwitchActive () {
        foreach (var item in quizList)
        {
            if (item != selectedQuiz)
            {
                foreach (var but in item.buttons)
                {
                    but.gameObject.SetActive(false);
                }
                foreach (var text in item.textGraphic)
                {
                    text.text.enabled = false;
                }
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
                foreach (var but in item.buttons)
                {
                    but.gameObject.SetActive(true);
                }
                foreach (var text in item.textGraphic)
                {
                    text.text.enabled = true;
                }
                if (player.frame != item.pauseFrame)
                {
                    player.frame = item.pauseFrame;
                    
                }
                player.Pause();
            }
        }
	}

    public static Quiz FindQuiz(string name)
    {
        return quizList.Find(x => x.name == name);
    }

    public void CreateQuiz(string pName)
    {
        GameObject tmpObj = new GameObject("Quiz");
        Quiz tmp=tmpObj.AddComponent<Quiz>();
        tmp.name = pName;
        tmp.pauseFrame = player.frame;
        player.Pause();
        
        QuizManager.quizList.Add(tmp);
        //Quiz.count = QuizManager.quizList.Count;
        menu.RefreshQuizList();
        //GameObject.Destroy(tmp);
    }

    public void DeleteQuiz()
    {
        if (quizList.Count > 0)
        {
            if (selectedQuiz != null)
            {
                foreach (var item in selectedQuiz.buttons)
                {
                    GameObject.Destroy(item.gameObject);
                }
                quizList.Remove(selectedQuiz);
                menu.DeleteItem();
            }
            else
            {
                StartCoroutine(QuizTween(selectQuizTween));
            }
        }
        else
        {
            StartCoroutine(QuizTween(createQuizTween));
        }
    }

    IEnumerator QuizTween(EasyTween tween)
    {
        yield return new WaitForEndOfFrame();
        tween.OpenCloseObjectAnimation();
        yield return new WaitForSeconds(1f);
        tween.OpenCloseObjectAnimation();
    }
}
