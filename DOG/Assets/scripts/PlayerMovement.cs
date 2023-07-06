using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float velocity = 5f;
    [SerializeField] float turnSpeed = 10;
    [SerializeField] LayerMask wall;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform gameObj;
    [SerializeField] Transform deadCheck;
    public Vector3 infSize;
    float based;
    float doubled;
    float halved;
    bool isSprinting;
    bool isSneaking;
    public bool attacked;
    

    Vector2 input;
    float angle;
    Vector3 lastPosition;

    Quaternion targetRotation;
    Transform cam;
    
    void Awake()
    {
        based = velocity;
        doubled = velocity*2;
        halved = velocity*0.5f;
        isSprinting = false;
        isSneaking = false;
    }
    void Start()
    {
        cam = Camera.main.transform;
        lastPosition = transform.position;
    }

    void Update()
    {
        GetInput();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

        CalculateDirection();
        Rotation();
        
        if ( transform.position == lastPosition ){
            StartCoroutine("size", 0.5);
            Debug.Log("wow");
        }
        Debug.Log(lastPosition);
        Debug.Log(transform.position);
        lastPosition = transform.position;
        if (!attacked)
        {
            Move();
            if ((Input.GetKey("left shift") || Input.GetKey("joystick button 0")) && !isSneaking){
                velocity = doubled;
                StartCoroutine("size", 1.5);
                isSprinting = true;
            }
            else if ((Input.GetKey("left ctrl") || Input.GetKey("joystick button 2")) && !isSprinting){
                velocity = halved;
                StartCoroutine("size", 0.5);
                isSneaking = true;
            }
            else
            {
                velocity = based;
                StartCoroutine("size", 1);
                isSneaking = false;
                isSprinting = false;
            }
        }
        if (attacked)
        {
            
        }
        walled();
        gameObj.transform.eulerAngles = new Vector3(
            gameObj.transform.eulerAngles.x * 0,
            gameObj.transform.eulerAngles.y * 0,
            gameObj.transform.eulerAngles.z * 0
        );
        gameObj.localScale = infSize;
        
    }

    void GetInput()
    {


        if (Input.GetAxisRaw("Horizontal")>0.4){
            input.x = 1;
        }
        else if (Input.GetAxisRaw("Horizontal")<-0.4){
            input.x = -1;
        }
        else
        {
            input.x=0;
        }
        if (Input.GetAxisRaw("Vertical")>0.4){
            input.y = 1;
        }
        else if (Input.GetAxisRaw("Vertical")<-0.4){
            input.y = -1;
        }
        else
        {
            input.y=0;
        }

    }
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x,input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;
        
    }
    void Rotation()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
    void Move()
    {
        transform.position += transform.forward * velocity * Time.deltaTime;
    }
    void walled()
    {
        if (Physics.CheckSphere(wallCheck.position, .2f, wall))
        {
            transform.position -= transform.forward * velocity * Time.deltaTime * 2;
        }
    }
    IEnumerator size(float target)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetSize = new Vector3(target,1,target);
        float diff = Vector3.Distance(infSize,targetSize);
        while (diff > 0.02f)
        {
            infSize = Vector3.SmoothDamp(infSize, targetSize, ref vel, 0.5f);
            diff = Vector3.Distance(infSize, targetSize);
            yield return null;
        }
        infSize = targetSize;

    }
    private void OnTrigger(Collider other)
    {
        if (other.gameObject.CompareTag("coyote"))
        {
            attacked=true;
        }
    }
}
