using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu; 
    public PlayerMovement obi;

    void Update(){
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))){
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);
            obi.CanMove = !obi.CanMove;
        }
    }
}
