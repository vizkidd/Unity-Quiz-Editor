using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreateTextDynamic : MonoBehaviour {
    enum State { dummy, CreatingTextBox, AdjustingSize, ConfigScreen, End };
    State currentState;
    bool textOnObject,curState,prevState;
    GameObject tmp,tmpTextObject;
    Text tmpText;
    RectTransform tmpImageTransform, tmpTextTransform;
    public GameObject configCanvas,textCanvas;
    private Vector3[] vertex;
    public Texture2D cursor;
    public EasyTween createQuizTween, selectQuizTween;
    public Sprite boundsSprite;
    static VideoPlayer player;
    float width, height;
    Button selectedButton;
    Image tmpGraphic;
    CircleOutline tmpOutline;
    Shadow tmpShadow;
    FlexibleResizeHandler resizer;
    public Text configCanvasText;
    public CircleOutline configCanvasTextOutline;
    public Shadow configCanvasShadow;
    public Image configTextImage;
    public RectTransform configInput;
    //LineRenderer lineObjRend;
    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<VideoPlayer>();
        vertex = new Vector3[4];
         /*Resources.LoadAll("", typeof(Font));
        foreach (Font item in Resources.FindObjectsOfTypeAll<Font>())
        {
            Debug.Log(item.name);
        }*/
    }
	
	// Update is called once per frame
	void Update () {
        if (currentState == State.CreatingTextBox)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            prevState = curState;
            curState = Input.GetMouseButtonDown(0);
            if (curState != prevState && curState == true)
            {
                tmp = new GameObject("Image");
                tmpTextObject = new GameObject("Text");
                tmpTextObject.tag = "Text";
                tmpTextObject.transform.SetParent(tmp.transform);
                tmpImageTransform = tmp.AddComponent<RectTransform>();
                tmpTextTransform = tmpTextObject.AddComponent<RectTransform>();
                tmpOutline = tmpTextObject.AddComponent<CircleOutline>();
                tmpShadow = tmpTextObject.AddComponent<Shadow>();
                //tmpImageTransform.anchorMin = tmpImageTransform.anchorMax = new Vector2(0, 1);
                tmpImageTransform.pivot = new Vector2(0,1);
                tmp.transform.position = Input.mousePosition;
                tmpGraphic = tmp.AddComponent<Image>();
                tmpGraphic.type = Image.Type.Sliced;
                tmpGraphic.raycastTarget = false;
                tmpGraphic.sprite = boundsSprite;
                tmp.transform.SetParent(textCanvas.transform);
                
                /*tmpTransform.GetWorldCorners(vertex);

                vertex[3].z =vertex[2].z= vertex[1].z =vertex[0].z = 0f;*/
                pointerData.position = Input.mousePosition;

                /* List<RaycastResult> results = new List<RaycastResult>();

               EventSystem.current.RaycastAll(pointerData, results);
             foreach (var item in results)
               {
                   if (item.gameObject.tag == "Answer")
                   {
                       selectedButton=item.gameObject.GetComponent<Button>();
                       selectedButton.interactable = false;
                       //tmp.transform.SetParent(item.gameObject.transform);
                       tmp.transform.SetParent(boundsCanvas.transform);
                   }
               }*/
                currentState = State.AdjustingSize;
            }
        }
        if (currentState == State.AdjustingSize)
        {
            /*tmpTransform.GetWorldCorners(vertex);
            vertex[3].z = vertex[2].z = vertex[1].z = vertex[0].z = 0f;*/
                prevState = curState;
            curState = Input.GetMouseButtonDown(0);

            if (Input.GetMouseButton(0))
            {
                

                width = Mathf.Abs(tmpImageTransform.position.x - Input.mousePosition.x);
                height = Mathf.Abs(tmpImageTransform.position.y - Input.mousePosition.y);
                tmpImageTransform.sizeDelta = new Vector2(width, height);
                tmpTextTransform.sizeDelta = new Vector2(width, height);
            }
            else
            {

                configInput.sizeDelta = configTextImage.rectTransform.sizeDelta = configCanvasText.rectTransform.sizeDelta = tmpTextTransform.sizeDelta;
                currentState = State.ConfigScreen;
            }
        }
        if(currentState == State.ConfigScreen)
        {
            StartCoroutine(DisableTextCanvasCO(textCanvas));
           StartCoroutine(EnableConfigCO(configCanvas));
        }
        if (currentState == State.End)
        {
            tmpText=tmpTextObject.AddComponent<Text>(configCanvasText);
            tmpText.color = configCanvasText.color;
            tmpText.supportRichText = true;
            if (configCanvasTextOutline.enabled)
            {
                tmpOutline.effectColor = configCanvasTextOutline.effectColor;
                tmpOutline.effectDistance = configCanvasTextOutline.effectDistance;
                tmpOutline.circleCount = configCanvasTextOutline.circleCount;
                tmpOutline.firstSample = configCanvasTextOutline.firstSample;
                tmpOutline.sampleIncrement = configCanvasTextOutline.sampleIncrement;
            }
            else
            {
                tmpOutline.enabled = false;
            }
            if (configCanvasShadow.enabled)
            {
                tmpShadow.effectColor = configCanvasShadow.effectColor;
                tmpShadow.effectDistance = configCanvasShadow.effectDistance;
            }
            else
            {
                tmpShadow.enabled = false;  
            }
            tmpTextObject.transform.SetParent(textCanvas.transform);
            QuizManager.selectedQuiz.textGraphic.Add(new TextGraphic(tmpText, tmpOutline, tmpShadow));
            GameObject.Destroy(tmp);
            GetComponent<Button>().interactable = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            currentState = State.dummy;
        }
    }
    IEnumerator DisableTextCanvasCO(GameObject can)
    {
        yield return new WaitForEndOfFrame();
        can.gameObject.SetActive(false);
    }
    IEnumerator EnableTextCanvasCO(GameObject can)
    {
        yield return new WaitForEndOfFrame();
        can.gameObject.SetActive(true);
    }
    IEnumerator EnableConfigCO(GameObject can)
    {
        yield return new WaitForEndOfFrame();
        can.gameObject.SetActive(true);
    }
    IEnumerator DisableConfigCO(GameObject can)
    {
        yield return new WaitForEndOfFrame();
        can.gameObject.SetActive(false);
    }
    public void DiscardChanges()
    {
        StartCoroutine(DisableConfigCO(configCanvas));
        selectedButton.interactable = true;
        currentState = State.End;
    }

    public void ApplyChanges()
    {
        StartCoroutine(DisableConfigCO(configCanvas));
        StartCoroutine(EnableTextCanvasCO(textCanvas));
        currentState = State.End;
        Debug.Log("Reached");
    }
    public void CreateTextBox()
    {

        if (QuizManager.selectedQuiz != null)
        {
            GetComponent<Button>().interactable = false;
            player.time = QuizManager.selectedQuiz.pauseFrame;
            player.Pause();
            Cursor.SetCursor(cursor, new Vector2(cursor.width / 2, cursor.height / 2), CursorMode.ForceSoftware);
            currentState = State.CreatingTextBox;
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

