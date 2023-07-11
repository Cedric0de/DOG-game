using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class influence : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunHash;
    int isSneakHash;
    public float growSpeed;
    bool isSprinting = false;
    bool isSneaking = false;
    public bool bushed = false;
    Vector3 infSize = new Vector3(0.5f,1f,0.5f);
    public PlayerMovement obi;
    public bool cantrun = false;
    [SerializeField]
    private staminabarUI staminaBar;
    int mash = 0;
    public GameObject button;
    public GameObject obi_final;
    public GameObject dust;
    //Vector3 growFast;

    // Start is called before the first frame update
    void Start()
    {
        staminaBar.SetMaxStamina(obi.maxStamina);
        animator = obi_final.GetComponent<Animator>();
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
        
        if(obi.attacked)
        {
            button.SetActive(true);
            dust.SetActive(true);
            obi_final.SetActive(false);
            
        }
        else
        {
            button.SetActive(false);
            dust.SetActive(false);
            obi_final.SetActive(true);


        }
        staminaBar.SetStamina(obi.stamina);
        if(obi.stamina<=0)
        {
            cantrun=true;
        }
        if(cantrun){
            if(obi.stamina>=obi.maxStamina){
                cantrun=false;
            }
        }
        if(!obi.attacked && !bushed)
        {
            if(obi.CanMove){
                if((Input.GetAxisRaw("Horizontal")>0.4) || (Input.GetAxisRaw("Vertical")>0.4) || (Input.GetAxisRaw("Horizontal")<-0.4) || (Input.GetAxisRaw("Vertical")<-0.4))
                {
                    animator.SetBool(isWalkingHash, true);
                    if ((Input.GetKey("left shift") || Input.GetKey("joystick button 0")) && !isSneaking && !cantrun){
                            grow(2f);
                            isSprinting = true;
                            animator.SetBool(isRunHash, true);
                        }
                        else if ((Input.GetKey("left ctrl") || Input.GetKey("joystick button 2")) && !isSprinting){
                            grow(0.8f);
                            isSneaking = true;
                            animator.SetBool(isSneakHash, true);
                            if (obi.stamina<obi.maxStamina)
                            {
                                obi.stamina += 4*Time.deltaTime;
                            }
                        }
                        else
                        {
                            grow(1.5f);
                            isSneaking = false;
                            isSprinting = false;
                            animator.SetBool(isSneakHash, false);
                            animator.SetBool(isRunHash, false);
                            if (obi.stamina<obi.maxStamina)
                            {
                                obi.stamina += 2*Time.deltaTime;
                            }
                        }
                    }
                else
                {
                    animator.SetBool(isWalkingHash, false);
                    grow(0.5f);
                    if (obi.stamina<obi.maxStamina)
                    {
                        obi.stamina += 4*Time.deltaTime;
                    }
                }
            }
        }
        else if(bushed && !obi.attacked)
        {
            grow(0f);
            if(obi.stamina<obi.maxStamina)
            {
                obi.stamina += 4*Time.deltaTime;
            }
        }
        else{
            grow(0f);
        }
        transform.localScale = infSize;
        if(obi.attacked)
        {
            if(mash<=0)
            {
                mash += Random.Range(8,15);
            }
            if(mash>0)
            {
                if(Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("space"))
                {
                    mash-=1;
                }
            }
            if(mash<=0)
            {
                obi.attacked = false;
            }
        }
        dust.transform.eulerAngles = new Vector3(
            dust.transform.eulerAngles.x * 0,
            dust.transform.eulerAngles.y * 0,
            dust.transform.eulerAngles.z * 0
        );
    }
    void grow(float size)
    {
        Vector3 target = new Vector3(size,0,size);
        float dist = Vector3.Distance(target,infSize);
        if(dist>1f)
        {
            if(size>transform.localScale.x)
                infSize += new Vector3(growSpeed*Time.smoothDeltaTime,0,growSpeed*Time.smoothDeltaTime);
            if(size<transform.localScale.x)
                infSize -= new Vector3(growSpeed*Time.smoothDeltaTime,0,growSpeed*Time.smoothDeltaTime);
        }
    }
}
