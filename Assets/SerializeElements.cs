using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

public class SerializeElements : MonoBehaviour {
    XmlWriterSettings settings;
    public static int quizCount=0;
    private bool readfail;
    private VideoPlayer player;

    //public static Quiz quiz;

    private void Start()
    {
        player = GameObject.FindObjectOfType<VideoPlayer>();
        settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = Encoding.UTF8;
        settings.NewLineOnAttributes = true;
    }
  
    public void SerializeQuiz()
    {
        using (XmlWriter writer = XmlWriter.Create("quiz.xml",settings))
        {
            writer.WriteStartDocument();

            ///
            writer.WriteStartElement("Root");
            if (player.source == VideoSource.Url)
                writer.WriteAttributeString("URL", player.url);
            else
            {
                writer.WriteAttributeString("Video", player.clip.name);
                writer.WriteAttributeString("Path",player.clip.originalPath);
            }
            writer.WriteAttributeString("Count", QuizManager.quizList.Count.ToString());
            foreach (Quiz quiz in QuizManager.quizList)
            {
                writer.WriteStartElement("Quiz");
                writer.WriteAttributeString("name", quiz.name);
                writer.WriteElementString("pause", quiz.pauseFrame.ToString());
                writer.WriteStartElement("Answers");
                foreach (var index in quiz.correctAnsIndex)
                {
                    writer.WriteElementString("Answer", index.ToString());
                }
                writer.WriteStartElement("Buttons");
                writer.WriteAttributeString("count", quiz.buttons.Count.ToString());
                foreach (Button button in quiz.buttons as List<Button>)
                {

                    writer.WriteStartElement("Button");
                    writer.WriteAttributeString("index", quiz.buttons.IndexOf(button).ToString());

                  RectTransform rT = button.GetComponent<RectTransform>();
                    SerializableVector2 sPosition = new SerializableVector2(rT.anchoredPosition.x, rT.anchoredPosition.y);
                    SerializableVector2 sAnchorMin = new SerializableVector2(rT.anchorMin.x, rT.anchorMin.y);
                    SerializableVector2 sAnchorMax = new SerializableVector2(rT.anchorMax.x, rT.anchorMax.y);
                    SerializableVector2 sPivot = new SerializableVector2(rT.pivot.x, rT.pivot.y);
                    SerializableVector2 sDelta = new SerializableVector2(rT.sizeDelta.x, rT.sizeDelta.y);
                    writer.WriteElementString("Position", sPosition.ToString());
                    writer.WriteElementString("AnchorMin", sAnchorMin.ToString());
                    writer.WriteElementString("AnchorMax", sAnchorMax.ToString());
                    writer.WriteElementString("Pivot", sPivot.ToString());
                    writer.WriteElementString("Size", sDelta.ToString());
                    writer.WriteEndElement();
                }
                foreach (TextGraphic item in quiz.textGraphic)
                {
                    writer.WriteStartElement("TextGraphic");
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}
