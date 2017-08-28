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
                writer.WriteEndElement();
                writer.WriteStartElement("Text");
                foreach (TextGraphic item in quiz.textGraphic)
                {
                    RectTransform trT = item.text.rectTransform;
                    SerializableVector2 sPosition = new SerializableVector2(trT.anchoredPosition.x, trT.anchoredPosition.y);
                    SerializableVector2 sDelta = new SerializableVector2(trT.sizeDelta.x, trT.sizeDelta.y);
                    SerializableColor tsColor = new SerializableColor(item.text.color.r, item.text.color.g, item.text.color.b, item.text.color.a);
                  
                    writer.WriteStartElement("TextGraphic");
                    writer.WriteAttributeString("bestfit",item.text.resizeTextForBestFit.ToString());
                    writer.WriteAttributeString("outline", item.outline.enabled.ToString());
                    writer.WriteAttributeString("shadow", item.outline.enabled.ToString());
                    writer.WriteElementString("Position", sPosition.ToString());
                    writer.WriteElementString("Size", sDelta.ToString());
                    writer.WriteElementString("Text", item.text.text);
                    writer.WriteElementString("FontSize", item.text.fontSize.ToString());
                    writer.WriteElementString("HOverflow", ((int)item.text.horizontalOverflow).ToString());
                    writer.WriteElementString("Color", tsColor.ToString());
                    writer.WriteElementString("Style", ((int)item.text.fontStyle).ToString());
                    writer.WriteElementString("Align", ((int)item.text.alignment).ToString());
                    if (item.outline.enabled)
                    {
                        SerializableVector2 osWidth = new SerializableVector2(item.outline.effectDistance.x, item.outline.effectDistance.y);
                        SerializableColor osColor = new SerializableColor(item.outline.effectColor.r, item.outline.effectColor.g, item.outline.effectColor.b, item.outline.effectColor.a);
                        writer.WriteStartElement("Outline");
                        writer.WriteElementString("Width", osWidth.ToString());
                        writer.WriteElementString("Color", osColor.ToString());
                        writer.WriteEndElement();
                    }
                    if (item.shadow.enabled)
                    {
                        SerializableVector2 ssWidth = new SerializableVector2(item.shadow.effectDistance.x, item.shadow.effectDistance.y);
                        SerializableColor ssColor = new SerializableColor(item.shadow.effectColor.r, item.shadow.effectColor.g, item.shadow.effectColor.b, item.shadow.effectColor.a);
                        writer.WriteStartElement("Shadow");
                        writer.WriteElementString("Width", ssWidth.ToString());
                        writer.WriteElementString("Color", ssColor.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
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
