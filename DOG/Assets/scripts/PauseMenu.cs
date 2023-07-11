using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu; 
    public GameObject obi;
    private PlayerMovement moveScript;

    void Start(){
        moveScript = obi.GetComponent<PlayerMovement>();
    }

    void Update(){
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKey("joystick button 7"))){
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);
            moveScript.CanMove = !moveScript.CanMove;
        }
    }
}
