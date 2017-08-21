using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetQuizNameString : MonoBehaviour {
    public InputField field;
	// Use this for initialization
	void Awake () {
        
        GetComponent<Button>().onClick.AddListener(() => { GameObject.FindObjectOfType<QuizManager>().CreateQuiz(field.text); });
	}
	
}
