using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class rabbitCounter : MonoBehaviour
{
    public int rabbits;
    public Text counter;
    int maxRabbit;
    public bool win;
    // Start is called before the first frame update
    void Start()
    {
        maxRabbit = 20;
        win = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateCounter(rabbits,maxRabbit);
        if(rabbits==maxRabbit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            win = true;
        }
    }
    void updateCounter(int bunnies, int max)
    {
        counter.text = string.Format("{0}/{1}",bunnies,max);
    }
}
