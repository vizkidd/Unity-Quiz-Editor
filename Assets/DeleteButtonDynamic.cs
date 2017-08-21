using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteButtonDynamic : MonoBehaviour {
    public Texture2D deleteCursor;
    enum State { Deleting, End, dummy };
    State currentState;

    public GameObject info;

    public EasyTween createQuizTween, selectQuizTween;

    // Use this for initialization
    void Start () {
        currentState = State.dummy;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Camera.main.ScreenPointToRay(Input.mousePosition).ToString());
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

      

        if (currentState == State.Deleting)
        {
            if(Input.GetMouseButtonDown(0))
            {
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointerData, results);

                foreach (var item in results)
                {
                    if (item.gameObject.tag=="Answer")
                    {
                        //var index = QuizManager.selectedQuiz.buttons.IndexOf(item.gameObject.transform.GetComponent<Button>());
                        //if (QuizManager.selectedQuiz.checkmarks[index] != null)
                        //{ QuizManager.selectedQuiz.checkmarks.RemoveAt(index); }

                        QuizManager.selectedQuiz.buttons.Remove(item.gameObject.transform.GetComponent<Button>());
                        GameObject.Destroy(item.gameObject);

                    }
                    
                }

                    
            }
            if (Input.GetMouseButtonDown(1))
            {
                currentState = State.End;
            }
        }
        if(currentState== State.End)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            currentState = State.dummy;
            info.SetActive(false);
        }
	}
    public void DeleteButton()
    {

        if (QuizManager.selectedQuiz != null)
        {
            if (currentState != State.Deleting)
            {
                Cursor.SetCursor(deleteCursor, new Vector2(deleteCursor.width / 2, deleteCursor.height / 2), CursorMode.ForceSoftware);
                currentState = State.Deleting;
                info.SetActive(true);
            }
            else
            {
                currentState = State.End;
                info.SetActive(false);
            }
        }
        else if (QuizManager.quizList.Count>0)
        {
            if (!SwitchView.show)
            {
                GameObject.FindObjectOfType<SwitchView>().OnClick();
            }
            StartCoroutine(QuizTween(selectQuizTween));

        }
        else
        { StartCoroutine(QuizTween(createQuizTween)); }
    }

    IEnumerator QuizTween(EasyTween tween)
    {
        yield return new WaitForEndOfFrame();
        tween.OpenCloseObjectAnimation();
        yield return new WaitForSecondsRealtime(1f);
        tween.OpenCloseObjectAnimation();
    }
}
