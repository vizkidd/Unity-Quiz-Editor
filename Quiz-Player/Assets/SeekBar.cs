using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SeekBar : MonoBehaviour {
    public static Slider seeker;
    static VideoPlayer player;
    public static double endTime;
    bool valueChanged;
    
    void Start() {
        seeker = GetComponent<Slider>();
        player = GameObject.FindObjectOfType<VideoPlayer>();



        seeker.maxValue = player.frameCount;
        player.seekCompleted += ContinuePlayback;
    }
    void ContinuePlayback(VideoPlayer player)
    {
        valueChanged = false;
        if (player.isPlaying)
        { player.Play(); }
    }
    // Update is called once per frame
    void Update() {
        //Debug.Log(player.frameRate);
        
        //Debug.Log(seeker.normalizedValue);
        if (!valueChanged)
            seeker.value = (float)(player.frame);
        else
           JumpToFrame(seeker.value);
    }
    public void ValueChanged()
    {
        valueChanged = true;

        //player.time = seeker.value * player.clip.length;

        JumpToFrame(seeker.value);

    }
    public static void JumpToFrame(float frame)
    {
        player.frame = (long)frame;
    }
    public void StopValueChange()
    {
        ContinuePlayback(player);
    }

}
