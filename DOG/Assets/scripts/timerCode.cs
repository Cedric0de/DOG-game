using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerCode : MonoBehaviour
{
    public rabbitCounter rabbits;
    public static float time;
    public Text timer;
    // Start is called before the first frame update
    void Start()
    {
        time=0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rabbits.win)
        {
            time += Time.deltaTime;
            updateTimer(time);
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
}
