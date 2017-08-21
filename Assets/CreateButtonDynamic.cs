using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CreateButtonDynamic : MonoBehaviour {
    public Texture2D cursor;
    Vector2 position;
    enum State { dummy, CreatingButton, GettingPosition, GettingSize, End};
    State currentState;
    bool prevState, curState, buttonFlipX, buttonFlipY;
    GameObject tmp;
    Button tmpButton;
    Image tmpGraphic;
    RectTransform tmpTransform;
    public Sprite buttonSprite;
    public GameObject buttonCanvas;
    static VideoPlayer player;
    float width, height;

    public EasyTween selectQuizTween, createQuizTween;

    int index;

	// Use this for initialization
	void Start () {
        
        player = GameObject.FindObjectOfType<VideoPlayer>();

    }

    void SetAnswerIndex(int index,Button button)
    {
        Debug.Log(index);
        if (QuizManager.selectedQuiz.correctAnsIndex.Contains(index))
        {
            QuizManager.selectedQuiz.correctAnsIndex.Remove(index);
            var colors = button.colors;
            colors.highlightedColor = Color.red;
            colors.pressedColor = Color.red;
            button.colors = colors;
        }
        else
        {
            QuizManager.selectedQuiz.correctAnsIndex.Add(index);
            var colors = button.colors;
            colors.highlightedColor =  Color.green;
            colors.pressedColor = Color.green;
            button.colors = colors;
        }



    }
    // Update is called once per frame
    void Update()
    {
        if (currentState == State.GettingPosition)
        {
            prevState = curState;
            curState = Input.GetMouseButtonDown(0);
            if (curState != prevState && curState == true)
            {
                tmp = new GameObject("button");
                tmp.tag = "Answer";
                tmp.transform.SetParent(buttonCanvas.transform);
                tmpTransform=tmp.AddComponent<RectTransform>();
                tmpGraphic = tmp.AddComponent<Image>();
                tmpGraphic.sprite = buttonSprite;
                tmpGraphic.type = Image.Type.Sliced;
                tmpButton = tmp.AddComponent<Button>();
                tmpTransform.pivot = new Vector2(0, 1);
                position = Input.mousePosition;
                tmpButton.transform.position = position;

                QuizManager.selectedQuiz.buttons.Add(tmpButton);

                int index = QuizManager.selectedQuiz.buttons.IndexOf(tmpButton);
                QuizManager.selectedQuiz.buttons[index].onClick.AddListener(() => { SetAnswerIndex(index, QuizManager.selectedQuiz.buttons[index]); });

                Debug.Log(position);
                currentState = State.GettingSize;
            }
        }
        if (currentState == State.GettingSize)
        {
            if (Input.GetMouseButton(0))
            {
                //if mouse pos > trans pos, then subtract else add
                if(tmpTransform.position.x < Input.mousePosition.x)
                {
                    width = Mathf.Abs(tmpTransform.position.x - Input.mousePosition.x);
                    buttonFlipX = false;
                }
                else
                {
                    width = (Input.mousePosition.x - tmpTransform.position.x);
                    buttonFlipX = true;
                }
                if (tmpTransform.position.y > Input.mousePosition.y)
                {
                    height = Mathf.Abs(tmpTransform.position.y - Input.mousePosition.y);
                    buttonFlipY = false;
                }
                else
                {
                    height = (tmpTransform.position.y- Input.mousePosition.y);
                    buttonFlipY = true;
                }

              if(buttonFlipX && buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(1, 0);
                    width = -width;
                    height = -height;
                }
              else if(!buttonFlipX && buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(0, 0);
                    height = -height;
                }
              else if(buttonFlipX && !buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(1, 1);
                    width = -width;
                }
              else
                {
                    tmpTransform.pivot = new Vector2(0, 1);

                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    width = height;
                }
                tmpTransform.sizeDelta = new Vector2(width, height);

            }
            else
            { currentState = State.End; }
        }
        if(currentState == State.End)
        {
            GetComponent<Button>().interactable = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            currentState = State.dummy;
        }
    }
    public void CreateButton()
    {

        if (QuizManager.selectedQuiz != null)
        {
            GetComponent<Button>().interactable = false;
            player.time = QuizManager.selectedQuiz.pauseFrame;
            player.Pause();
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
            currentState = State.GettingPosition;
        }
        else if (QuizManager.quizList.Count>0)
        {
            Debug.Log("Select a quiz first!");
            if(!SwitchView.show)
            {
                GameObject.FindObjectOfType<SwitchView>().OnClick();
            }
            StartCoroutine(QuizTween(selectQuizTween));


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
        yield return new WaitForSecondsRealtime(1f);
        tween.OpenCloseObjectAnimation();
    }

}
