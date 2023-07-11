using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finalTime : MonoBehaviour
{
    public Text timer;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        time = timerCode.time;
        time+=1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timer.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
