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
    float based;
    float doubled;
    float halved;
    bool isSprinting;
    bool isSneaking;
    public bool attacked;
    public influence inf;
    public float maxStamina;
    public float stamina;
    

    Vector2 input;
    float angle;
    Vector3 lastPosition;

    Quaternion targetRotation;
    Transform cam;
    
    void Awake()
    {
        based = velocity*1.5f;
        doubled = velocity*2.5f;
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
        if (!attacked)
        {
            Move();
            if ((Input.GetKey("left shift") || Input.GetKey("joystick button 0")) && !isSneaking && !inf.cantrun){
                velocity = doubled;
                isSprinting = true;
                stamina -= 2*Time.deltaTime;
            }
            else if ((Input.GetKey("left ctrl") || Input.GetKey("joystick button 2")) && !isSprinting){
                velocity = halved;
                isSneaking = true;
            }
            else
            {
                velocity = based;
                isSneaking = false;
                isSprinting = false;
            }
        }
        walled();
        gameObj.transform.eulerAngles = new Vector3(
            gameObj.transform.eulerAngles.x * 0,
            gameObj.transform.eulerAngles.y * 0,
            gameObj.transform.eulerAngles.z * 0
        );
        
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bush"))
        {
            inf.bushed = true;
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("bush"))
        {
            inf.bushed = false;
        }
    }
}
