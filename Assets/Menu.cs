using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    //Resolution[] resolutions;
    List<GameObject> menuButtons;
    [SerializeField]
    Transform menuPanel;
    [SerializeField]
    GameObject buttonPrefab;

    // Use this for initialization
    void Start()
    {
        //resolutions = Screen.resolutions;

        menuButtons = new List<GameObject>();
        for (int i = 0; i < QuizManager.quizList.Count; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            //button.GetComponentInChildren<Text>().text = ResToString(resolutions[i]);
            button.GetComponentInChildren<Text>().text = QuizManager.quizList[i].name;
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(
                () => { SelectQuiz(index); }
                );
            button.transform.SetParent(menuPanel.transform);
            menuButtons.Add(button);
        }
    }
    public void DeleteItem()
    {

        RefreshQuizList();
    }
   public void RefreshQuizList()
    {
        foreach (var item in menuButtons)
        {
            GameObject.Destroy(item);
        }
        for (int i = 0; i < QuizManager.quizList.Count; i++)
        {
            GameObject button = (GameObject)Instantiate(buttonPrefab);
            //button.GetComponentInChildren<Text>().text = ResToString(resolutions[i]);
            button.GetComponentInChildren<Text>().text = QuizManager.quizList[i].name;
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(
                () => { SelectQuiz(index); }
                );
            button.transform.SetParent(menuPanel.transform);
            menuButtons.Add(button);
        }
    }
    void SelectQuiz(int index)
    {
        QuizManager.selectedQuiz = QuizManager.quizList[index];
        QuizManager.SwitchActive();
        
    }
   /* void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, false);
    }*/

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }
}
