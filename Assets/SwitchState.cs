using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchState : MonoBehaviour {
    public GameObject switchedObject;
    bool state;

    public void OnClick()
    {
        state = !state;
        StartCoroutine(Switch());
    }
    IEnumerator Switch()
    {
        yield return new WaitForEndOfFrame();
        switchedObject.SetActive(state);
        
    }
}
