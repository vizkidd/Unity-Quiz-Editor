using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class QuizManager : MonoBehaviour {
    XmlReaderSettings settings;
    static int quizCount;
    static List<Quiz> quizList;
    public static Quiz selectedQuiz;
    public Canvas buttonCanvas;
    VideoPlayer player;
    List<int> answerIndex;
    bool xmlRead, runOnce,playURL, playable;
    public double threshold;
    public EasyTween hideUI,continueTween;
    public string URL,videoPath;
    private void Start()
    {
        player = FindObjectOfType<VideoPlayer>();
        settings = new XmlReaderSettings();
        settings.IgnoreComments = settings.IgnoreWhitespace = true;
        quizList = new List<Quiz>();
        answerIndex = new List<int>();
        player.prepareCompleted += Player_prepareCompleted;
        StartCoroutine(Autoplay());
        xmlRead =ReadXML();
        if (playURL)
        {
            player.playOnAwake = true;
            player.source = VideoSource.Url;
            player.url = URL;
           
        }
        
    }

    private void Player_prepareCompleted(VideoPlayer source)
    {
        SeekBar.seeker.maxValue = player.frameCount;
        player.playOnAwake=playable = true;
    }

    IEnumerator Autoplay()
    {
        yield return new WaitUntil(() =>{
            if (!playURL)
                playable = true;
            return (xmlRead && playable); });
        Debug.Log("Playing");

        
         player.Play();
    }

    public void CheckAnswer()
    {
        if (selectedQuiz.buttons.Count>0)
        {
            if (!selectedQuiz.multipleAnswers)
            {
                if (answerIndex[0] == selectedQuiz.correctAnsIndex[0])
                {
                    selectedQuiz.passed = true;
                }
                else
                {
                    selectedQuiz.passed = false;
                }
            }
            else
            {
                answerIndex.Sort();
                if (answerIndex.Count == selectedQuiz.correctAnsIndex.Count)
                {
                    for (int i = 0; i < answerIndex.Count; i++)
                    {
                        if (answerIndex[i] == selectedQuiz.correctAnsIndex[i])
                        {
                            selectedQuiz.passed = true;
                        }
                        else
                        {
                            selectedQuiz.passed = false;
                        }
                    }
                }
                else
                {
                    selectedQuiz.passed = false;
                }
            }
            Debug.Log("Passed: " + selectedQuiz.passed);
        }
        selectedQuiz.attended = true;
        EndQuiz();
    }
    void EndQuiz()
    {
        continueTween.OpenCloseObjectAnimation();
        foreach (var textGr in selectedQuiz.textGraphic)
        {
            textGr.text.gameObject.SetActive(false);
        }
        foreach (var button in selectedQuiz.buttons)
        {
            button.gameObject.SetActive(false);
            SwitchCanvas(false);
        }
        selectedQuiz = null;
        
        hideUI.OpenCloseObjectAnimation();
        
        runOnce = false;
        player.Play();
        
    }
    void StartQuiz()
    {
        player.Pause();
        SeekBar.JumpToFrame(selectedQuiz.pauseFrame);
        
        hideUI.OpenCloseObjectAnimation();
        continueTween.OpenCloseObjectAnimation();
        foreach (var textGr in selectedQuiz.textGraphic)
        {
            textGr.text.gameObject.SetActive(true);
        }
        foreach (var button in selectedQuiz.buttons)
        {
            button.gameObject.SetActive(true);
            SwitchCanvas(true);
        }
        
    }

    private void Update()
    {
        switch (xmlRead)
        {
            case true:
                foreach (var quiz in quizList)
                {
                    if(player.frame>=quiz.pauseFrame && Math.Abs(player.frame-quiz.pauseFrame)<=threshold && (!quiz.attended/* || !quiz.passed*/) && !runOnce && selectedQuiz==null)
                    {
                        runOnce = true;
                        selectedQuiz = quiz;

                        Debug.Log(quiz.name);
                        StartQuiz();
                        //pause player, show canvas
                        
                    }
                }
                break;
            case false:
                //xmlRead failed
                break;
            default:
                break;
        }
    }

    IEnumerator SwitchCanvas(bool activate)
    {
        yield return new WaitForEndOfFrame();
        buttonCanvas.enabled = activate;
    }

    public bool ReadXML()
    {
        try
        {
            using (XmlReader reader = XmlReader.Create("quiz.xml"))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Root":
                                reader.MoveToFirstAttribute();
                                if(reader.Name== "URL")
                                {
                                    URL = Convert.ToString(reader.Value);
                                    playURL = true;
                                }
                                else
                                {
                                    //first attriute for video is the name of the video and it's safe to skip
                                    reader.MoveToNextAttribute();
                                    videoPath = Convert.ToString(reader.Value);
                                }
                                reader.MoveToNextAttribute();
                                quizCount = Convert.ToInt32(reader.Value);
                                
                                break;
                            case "Quiz":
                                ReadQuiz(reader);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        catch
        {
            return false;
        }

        foreach (var quiz in quizList)
        {
            foreach(var button in quiz.buttons)
            {
                button.gameObject.SetActive(false);
            }
            foreach (var textGr in quiz.textGraphic)
            {
                textGr.text.gameObject.SetActive(false);
            }
        }

        Debug.Log("End");
        return true;
    }
    HorizontalWrapMode DeserializeIntToWrapMode(int value)
    {
        switch (value)
        {
            case 0:
                return HorizontalWrapMode.Wrap;
            case 1:
                return HorizontalWrapMode.Overflow;
            default:
                return HorizontalWrapMode.Wrap;
        }
    }
    Color DeserializeStringToColor(string value)
    {
        string[] tokens = value.Split(new char[] { '[', ',', ' ', ']' }, StringSplitOptions.RemoveEmptyEntries);

        return new Color(float.Parse(tokens[0]), float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
    }
    Vector2 DeserializeStringToVector2(string value)
    {
        string[] tokens = value.Split(new char[] { '[', ',', ' ', ']' }, StringSplitOptions.RemoveEmptyEntries);

        return new Vector2(float.Parse(tokens[0]), float.Parse(tokens[1]));
    }

    void SetAnswerIndex(int index,bool multipleAns)
    {
        if (multipleAns)
        {
            if (!answerIndex.Contains(index))
            {
                answerIndex.Add(index);
            }
            else
            {
                answerIndex.Remove(index);
            }
        }
        else
        {
            answerIndex.Clear();
            answerIndex.Add(index);
        }
        Debug.Log(index);
    }

     void ReadQuiz(XmlReader reader)
    {
        Quiz tmp = new Quiz();

        reader.MoveToFirstAttribute();
        switch (reader.Name)
        {
            case "name":
                tmp.name = reader.Value;
                break;
            default:
                break;
        }
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "pause":


                        tmp.pauseFrame = reader.ReadElementContentAsLong();

                        break;
                    case "Answers":
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "Answer":
                                        tmp.correctAnsIndex.Add(reader.ReadElementContentAsInt());
                                        if (tmp.correctAnsIndex.Count > 1)
                                        {
                                            tmp.multipleAnswers = true;
                                        }
                                        break;

                                    case "Buttons":
                                        reader.MoveToFirstAttribute();
                                        int count = Convert.ToInt32(reader.Value);
                                        if(count>0)
                                        { while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                switch (reader.Name)
                                                {

                                                    case "Button":
                                                        reader.MoveToFirstAttribute();
                                                        int index = Convert.ToInt32(reader.Value);
                                                        GameObject tmpObject = new GameObject("button");
                                                        GameObject tmpButtonImg = new GameObject("image");

                                                        Button tmpButton = tmpObject.AddComponent<Button>();
                                                        RectTransform tmpRect = tmpObject.AddComponent<RectTransform>();
                                                        RectTransform tmpButtonTransform = tmpButtonImg.AddComponent<RectTransform>();
                                                        Image tmpImg = tmpObject.AddComponent<Image>();

                                                        tmpObject.transform.SetParent(buttonCanvas.transform);
                                                        tmpButton.onClick.AddListener(() => { SetAnswerIndex(index, tmp.multipleAnswers); });
                                                        //tmpObject.GetComponent<Image>().enabled = false;   
                                                        //tmpImg.type = Image.Type.Sliced;
                                                        tmpImg.color = new Color32(255, 255, 255, 255);
                                                        tmpImg.sprite = Resources.Load("square", typeof(Sprite)) as Sprite;
                                                        tmpObject.AddComponent<Outline>();
                                                        tmpButton.targetGraphic = tmpImg;

                                                        tmpButtonImg.transform.parent = tmpButton.transform;
                                                        tmpImg = tmpButtonImg.AddComponent<Image>();
                                                        tmpImg.type = Image.Type.Sliced;
                                                        tmpImg.sprite = Resources.Load("button", typeof(Sprite)) as Sprite;
                                                        tmpButtonImg.AddComponent<ImageBehaviour>();

                                                        while (reader.Read())
                                                        {
                                                            if (reader.NodeType == XmlNodeType.Element)
                                                            {
                                                                switch (reader.Name)
                                                                {
                                                                    case "Position":
                                                                        tmpRect.anchoredPosition = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        //tmpButtonTransform.anchoredPosition = tmpRect.anchoredPosition;
                                                                        break;
                                                                    case "AnchorMin":
                                                                        tmpRect.anchorMin = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        //tmpButtonTransform.anchorMin = tmpRect.anchorMin;
                                                                        break;
                                                                    case "AnchorMax":
                                                                        tmpRect.anchorMax = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        //tmpButtonTransform.anchorMax = tmpRect.anchorMax;
                                                                        break;
                                                                    case "Pivot":
                                                                        tmpRect.pivot = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        //tmpButtonTransform.pivot = tmpRect.pivot;
                                                                        break;
                                                                    case "Size":
                                                                        tmpRect.sizeDelta = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        tmpButtonTransform.sizeDelta = tmpRect.sizeDelta;
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }
                                                            else if (reader.NodeType == XmlNodeType.EndElement)
                                                            {

                                                                tmp.buttons.Add(tmpButton);
                                                                break;
                                                            }
                                                        }
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                           else if (reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                break;
                                            }
                                        }
                                } 
                                        break;
                                    case "Text":
                                        while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                switch (reader.Name)
                                                {

                                                    case "TextGraphic":
                                                        reader.MoveToFirstAttribute();
                                                        bool bestFit = Convert.ToBoolean(reader.Value);
                                                        reader.MoveToNextAttribute();
                                                        bool outline = Convert.ToBoolean(reader.Value);
                                                        reader.MoveToNextAttribute();
                                                        bool shadow = Convert.ToBoolean(reader.Value);
                                                        GameObject tmpObject = new GameObject("text");
                                                        RectTransform tmpRect = tmpObject.AddComponent<RectTransform>();
                                                        Text tmpText = tmpObject.AddComponent<Text>();

                                                        tmpText.raycastTarget = false;

                                                        tmpText.font = (Font)Resources.Load("Fonts/arial");

                                                        CircleOutline tmpOutline = tmpObject.AddComponent<CircleOutline>();
                                                        Shadow tmpShadow = tmpObject.AddComponent<Shadow>();

                                                        tmpObject.transform.SetParent(buttonCanvas.transform);

                                                        if (bestFit)
                                                        {
                                                            tmpText.resizeTextForBestFit = true;
                                                        }
                                                        else
                                                            tmpText.resizeTextForBestFit = false;

                                                        if (outline)
                                                            tmpOutline.enabled = true;
                                                        else
                                                            tmpOutline.enabled = false;

                                                        if (shadow)
                                                            tmpShadow.enabled = true;
                                                        else
                                                            tmpShadow.enabled = false;

                                                        

                                                        while (reader.Read())
                                                        {
                                                            if (reader.NodeType == XmlNodeType.Element)
                                                            {
                                                                switch (reader.Name)
                                                                {
                                                                    case "Position":
                                                                        tmpRect.anchoredPosition = DeserializeStringToVector2(reader.ReadElementContentAsString());

                                                                        break;
                                                                    case "Size":
                                                                        tmpRect.sizeDelta = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                        
                                                                        break;
                                                                    case "Text":
                                                                        tmpText.text = reader.ReadElementContentAsString();
                                                                        break;
                                                                    case "FontSize":
                                                                        tmpText.fontSize = reader.ReadElementContentAsInt();
                                                                        tmpText.resizeTextMaxSize = tmpText.fontSize;
                                                                        break;
                                                                    case "Color":
                                                                        tmpText.color = DeserializeStringToColor(reader.ReadElementContentAsString());
                                                                        break;
                                                                    case "HOverflow":
                                                                        tmpText.horizontalOverflow = DeserializeIntToWrapMode(reader.ReadElementContentAsInt());
                                                                        break;
                                                                    case "Style":
                                                                        tmpText.fontStyle = DeserializeIntToStyle(reader.ReadElementContentAsInt());
                                                                        break;
                                                                    case "Align":
                                                                        tmpText.alignment = DeserializeIntToAlignment(reader.ReadElementContentAsInt());
                                                                        break;
                                                                    case "Outline":
                                                                        while (reader.Read())
                                                                        {
                                                                            if (reader.NodeType == XmlNodeType.Element)
                                                                            {
                                                                                switch (reader.Name)
                                                                                {
                                                                                    case "Width":
                                                                                        tmpOutline.effectDistance = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                                        break;
                                                                                    case "Color":
                                                                                        tmpOutline.effectColor = DeserializeStringToColor(reader.ReadElementContentAsString());
                                                                                        break;
                                                                                    default:
                                                                                        break;
                                                                                }
                                                                            }
                                                                            else if (reader.NodeType == XmlNodeType.EndElement)
                                                                            { break; }
                                                                        }
                                                                                break;
                                                                    case "Shadow":
                                                                        while (reader.Read())
                                                                        {
                                                                            if (reader.NodeType == XmlNodeType.Element)
                                                                            {
                                                                                switch (reader.Name)
                                                                                {
                                                                                    case "Width":
                                                                                        tmpShadow.effectDistance = DeserializeStringToVector2(reader.ReadElementContentAsString());
                                                                                        break;
                                                                                    case "Color":
                                                                                        tmpShadow.effectColor = DeserializeStringToColor(reader.ReadElementContentAsString());
                                                                                        break;
                                                                                    default:
                                                                                        break;
                                                                                }
                                                                            }
                                                                            else if (reader.NodeType == XmlNodeType.EndElement)
                                                                            { break; }
                                                                        }
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                            }
                                                            else if (reader.NodeType == XmlNodeType.EndElement)
                                                            {

                                                                //add text into list
                                                                tmp.textGraphic.Add(new TextGraphic(tmpText, tmpOutline, tmpShadow));
                                                                break;
                                                            }
                                                        }

                                                        
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                            else if (reader.NodeType == XmlNodeType.EndElement)
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    default:

                                        break;
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                break;
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else if (reader.NodeType == XmlNodeType.EndElement)
            {
                quizList.Add(tmp);
                break;
            }
        }
    }

    private TextAnchor DeserializeIntToAlignment(int v)
    {
        switch (v)
        {
            case 0:
                return TextAnchor.UpperLeft;
            case 1:
                return TextAnchor.UpperCenter;
            case 2:
                return TextAnchor.UpperRight;
            case 3:
                return TextAnchor.MiddleLeft;
            case 4:
                return TextAnchor.MiddleCenter;
            case 5:
                return TextAnchor.MiddleRight;
            case 6:
                return TextAnchor.LowerLeft;
            case 7:
                return TextAnchor.LowerCenter;
            case 8:
                return TextAnchor.LowerRight;
            default:
                return TextAnchor.MiddleCenter; 
        }
    }

    private FontStyle DeserializeIntToStyle(int v)
    {
        switch (v)
        {
            case 0:
                return FontStyle.Normal;
            case 1:
                return FontStyle.Bold;
            case 2:
                return FontStyle.Italic;
            case 3:
                return FontStyle.BoldAndItalic;
            default:
                return FontStyle.Normal;
        }
    }
}
