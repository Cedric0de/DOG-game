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
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
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
