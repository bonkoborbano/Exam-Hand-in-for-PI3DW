using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour
{
    public static TimerController instance;
    
    public Text timerCounter;

    private TimeSpan timePlaying;
    private bool timerGoing;
    
    private float elapsedTime;
    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        timerCounter.text = "Time: 00:00.00";
        timerGoing = false;
    }
    
    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;
        
        StartCoroutine(UpdateTimer());
    }
    
    public void EndTimer()
    {
        timerGoing = false;
    }
    
    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
            timerCounter.text = timePlayingStr;
            
            yield return null;
        }
    }
}
