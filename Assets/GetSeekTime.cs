using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GetSeekTime : MonoBehaviour {
    VideoPlayer player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = ((float)player.time).ToString("n2");
	}
}
