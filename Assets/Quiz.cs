using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TextGraphic
{
    public Text text;
    public CircleOutline outline;
    public Shadow shadow;
    public TextGraphic(Text t, CircleOutline o, Shadow s)
    {
        text = t;
        outline = o;
        shadow = s;
    }
}
public class Quiz:MonoBehaviour {
  
    public string name;
    //public int index;
    public long pauseFrame;
    public List<TextGraphic> textGraphic;
    //public static int count=0;
    //public List<Image> checkmarks;
    public List<Button> buttons;
   public List<int> correctAnsIndex;
    private void Start()
    {
        correctAnsIndex = new List<int>();
        buttons = new List<Button>();
        textGraphic = new List<TextGraphic>();
        //checkmarks = new List<Image>();
    }
	public Quiz()
    {
        correctAnsIndex = new List<int>();
        buttons = new List<Button>();
        textGraphic = new List<TextGraphic>();
        //checkmarks = new List<Image>();
    }
}
