using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFontControl : MonoBehaviour {
    public List<Font> fonts;
	// Use this for initialization
	void Start () {
        fonts = new List<Font>();
      foreach (Font item in Resources.FindObjectsOfTypeAll<Font>())
      {
            fonts.Add(item);
      }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
