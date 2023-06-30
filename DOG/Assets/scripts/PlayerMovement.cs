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
    //PlayerControls controls;

    Vector2 input;
    float angle;

    Quaternion targetRotation;
    Transform cam;
    
    void Awake()
    {
        /*
        controls = new PlayerControls();

        controls.Gameplay.Grow.performed += ctx => Grow();

        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        */
    }
    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        GetInput();

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

        CalculateDirection();
        Rotation();
        Move();
        walled();
        gameObj.transform.eulerAngles = new Vector3(
            gameObj.transform.eulerAngles.x * 0,
            gameObj.transform.eulerAngles.y * 0,
            gameObj.transform.eulerAngles.z * 0
        );
        /*
        move = controls.Gameplay.Move.ReadValue<Vector2>();
        Vector2 m = new Vector2(move.x,move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);
        */
    }
    /*
    void Grow()
    {
        transform.localScale *= 1.1f;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    */
    void GetInput()
    {
        // input.x = Input.GetAxisRaw("Horizontal");
        // input.y = Input.GetAxisRaw("Vertical");


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
        Debug.Log((Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
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
}
