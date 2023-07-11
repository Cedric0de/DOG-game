using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunHash;
    int isSneakHash;
    bool isSprinting = false;
    bool isSneaking = false;
    public PlayerMovement obi;
    public bool cantrun = false;
    [SerializeField]
    private staminabarUI staminaBar;
    //Vector3 growFast;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunHash = Animator.StringToHash("isRun");
        isSneakHash = Animator.StringToHash("isSneak");
        //growFast = 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRun = animator.GetBool(isRunHash);
        bool isSneak = animator.GetBool(isSneakHash);
        if(obi.stamina<=0)
        {
            cantrun=true;
        }
        if(cantrun){
            if(obi.stamina>=obi.maxStamina){
                cantrun=false;
            }
        }
        if(!obi.attacked)
        {
            if(obi.CanMove){
                if((Input.GetAxisRaw("Horizontal")>0.4) || (Input.GetAxisRaw("Vertical")>0.4) || (Input.GetAxisRaw("Horizontal")<-0.4) || (Input.GetAxisRaw("Vertical")<-0.4))
                {
                    animator.SetBool(isWalkingHash, true);
                    if ((Input.GetKey("left shift") || Input.GetKey("joystick button 0")) && !isSneaking && !cantrun){

                            isSprinting = true;
                            animator.SetBool(isRunHash, true);
                        }
                        else if ((Input.GetKey("left ctrl") || Input.GetKey("joystick button 2")) && !isSprinting){

                            isSneaking = true;
                            animator.SetBool(isSneakHash, true);

                        }
                        else
                        {

                            isSneaking = false;
                            isSprinting = false;
                            animator.SetBool(isSneakHash, false);
                            animator.SetBool(isRunHash, false);

                        }
                    }
                else
                {
                    animator.SetBool(isWalkingHash, false);

                }
            }
        }
    }
}
