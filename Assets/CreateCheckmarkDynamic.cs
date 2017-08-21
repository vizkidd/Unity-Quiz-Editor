using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateCheckmarkDynamic : MonoBehaviour {
    enum State { dummy, Selecting, Clamping, Creating,End };
    State currentState;
    public Texture2D cursor;
    Vector2 position;
    bool prevState, curState, buttonFlipX, buttonFlipY;
    public static GameObject selectedButton;
    GameObject tmp;
    Image tmpGraphic, selectedButtonImg;
    public Sprite buttonSprite;
    RectTransform tmpTransform, buttonTransform;
    float width, height;
    public EasyTween selectQuizTween, createQuizTween;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentState == State.Selecting)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            prevState = curState;
            curState = Input.GetMouseButtonDown(0);
            if (curState != prevState && curState == true)
            {
                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointerData, results);

                foreach (var item in results)
                {
                    if (item.gameObject.tag == "Answer")
                    {
                        item.gameObject.GetComponent<Button>().interactable = false;
                        tmp = new GameObject("check");
                        tmp.tag = "Answer";
                        tmpTransform = tmp.AddComponent<RectTransform>();
                        selectedButton = item.gameObject;
                        buttonTransform = item.gameObject.GetComponent<RectTransform>();
                        selectedButtonImg = item.gameObject.GetComponent<Image>();
                        tmpGraphic = tmp.AddComponent<Image>();
                        tmpGraphic.sprite = buttonSprite;
                        tmpGraphic.type = Image.Type.Sliced;
                        tmpTransform.pivot = new Vector2(0, 1);
                        tmp.transform.SetParent(selectedButton.transform);

                      

                        currentState = State.Clamping;
                    }

                }
            }
        }
        if(currentState == State.Clamping)
        {
            if (Input.GetMouseButton(0))
            {

                position = Input.mousePosition;
                tmpTransform.position = position;
                currentState = State.Creating;
            }
        }
        if (currentState == State.Creating)
        {
            if (Input.GetMouseButton(0))
            {
                //if mouse pos > trans pos, then subtract else add
                if (tmpTransform.position.x < Input.mousePosition.x)
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
                    height = (tmpTransform.position.y - Input.mousePosition.y);
                    buttonFlipY = true;
                }
                if (width > height)
                {
                    width = height;
                }
                else
                {
                    height = width;
                }
                // Debug.Log("tmptrans: " + tmpTransform.offsetMax.x + " button trans: "+selectedButtonImg.rectTransform.offsetMax.x);

               /* float tempOffsetX = Mathf.Abs(selectedButtonImg.rectTransform.offsetMax.x - (Mathf.Abs(tmpTransform.position.x) - Mathf.Abs(selectedButtonImg.rectTransform.position.x)));
                float tempOffsetY = Mathf.Abs(Mathf.Abs(selectedButtonImg.rectTransform.offsetMax.y) - (Mathf.Abs(tmpTransform.position.y) - Mathf.Abs(selectedButtonImg.rectTransform.position.y)));

               if (tempOffsetX < tempOffsetY)
                {
                    tempOffsetY = tempOffsetX;
                }
                else
                {
                    tempOffsetX = tempOffsetY;
                }*/
               // Debug.Log(" offsetX: " + (tempOffsetX) + " offsetY: " + (tempOffsetY));
                //Debug.Log((selectedButtonImg.rectTransform.position.x + selectedButtonImg.rectTransform.rect.width) - (tmpTransform.position.x+tmpTransform.rect.width));
                //Debug.Log((selectedButtonImg.rectTransform.position.y + selectedButtonImg.rectTransform.rect.height) - (tmpTransform.position.y + tmpTransform.rect.height));

               if (buttonFlipX && buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(1, 0);
                    width = -width;
                    height = -height;
                }
                else if (!buttonFlipX && buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(0, 0);
                    height = -height;
                }
                else if (buttonFlipX && !buttonFlipY)
                {
                    tmpTransform.pivot = new Vector2(1, 1);
                    width = -width;
                }
                else
                {
                    tmpTransform.pivot = new Vector2(0, 1);

                }
                tmpTransform.sizeDelta = new Vector2(width, height);
                //tmpTransform.sizeDelta = new Vector2(Mathf.Clamp(width, 0, tempOffsetX), Mathf.Clamp(height, 0, tempOffsetY));

            }
            else
            {
                selectedButton.GetComponent<Button>().interactable = true;

                //QuizManager.selectedQuiz.checkmarks.Insert(QuizManager.selectedQuiz.buttons.IndexOf(selectedButton.GetComponent<Button>()), tmpGraphic);
                selectedButton = null;
                currentState = State.End; }
        }
        if (currentState == State.End)
        {
            GetComponent<Button>().interactable = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            currentState = State.dummy;
        }
    }

    public void CreateCheckmark()
    {

        if (QuizManager.selectedQuiz != null)
        {
            GetComponent<Button>().interactable = false;
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
            currentState = State.Selecting;
        }
        else if (QuizManager.quizList.Count > 0)
        {
            Debug.Log("Select a quiz first!");
            if (!SwitchView.show)
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
