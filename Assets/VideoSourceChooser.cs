using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSourceChooser : MonoBehaviour {
    public string URL = "file://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";   // Use this for initialization
    public VideoClip clip;
    public static VideoPlayer player;
    void Start () {
        player = GameObject.FindObjectOfType<VideoPlayer>();
        player.errorReceived += Player_errorReceived;
        player.prepareCompleted += Player_prepareCompleted;
        Debug.Log(Application.internetReachability);
        Debug.Log(Network.isClient);
    }

    private void Player_prepareCompleted(VideoPlayer source)
    {
        SeekBar.seeker.maxValue = player.frameCount;
        player.Play();
        
    }

    public void ChooseURL()
    {
        player.source = VideoSource.Url;
        player.url = URL;
        player.playOnAwake = true;
        player.Prepare();
        
        
    
        //SeekBar.endTime = (double)(1f / player.frameCount);
    }
    
    
    private void Player_errorReceived(VideoPlayer source, string message)

    {
        Debug.Log(message);
        Application.Quit();
    }

    public void ChooseVideo()
    {
        player.source = VideoSource.VideoClip;
        player.clip = clip;
        
        player.Prepare();
        SeekBar.seeker.maxValue = player.frameCount;
        player.Play();

    }
   
}
