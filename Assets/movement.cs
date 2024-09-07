using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class movement : MonoBehaviour
{

    public InputActionReference move;

    public InputActionReference cam;

    private Vector2 movementinput;

    private Vector2 caminput;

    public float maxspeed;

    public float speedvect;

    private Rigidbody2D RB2D;

    private float averagespeed;


    // Start is called before the first frame update
    void Start()
    {
        move.action.performed += OnMovementChange;

        cam.action.performed += OnCamChange;

        RB2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!move.action.IsPressed())
        {
            movementinput = Vector2.zero;
        }

        if (!cam.action.IsPressed())
        {
            caminput = Vector2.zero;
        }

        if(caminput!=Vector2.zero)
        {
            float angleA = 0f;
            if (Mathf.Atan2(caminput.x, caminput.y) * Mathf.Rad2Deg != 0)
            { 
                angleA = Mathf.Atan2(caminput.x, -caminput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }
        }
        else
        {
            float angleA = 0f;
            if (Mathf.Atan2(movementinput.x, movementinput.y) * Mathf.Rad2Deg != 0)
            {
                angleA = Mathf.Atan2(movementinput.x, -movementinput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }
        }

        averagespeed = (float)(Mathf.Abs(RB2D.velocity.x) + Mathf.Abs(RB2D.velocity.y))/2f;

        if(movementinput != null && averagespeed < maxspeed)
        {
            RB2D.AddForce(movementinput*speedvect);
        }
        //if(movementinput.x==0)
        //{
        //    RB2D.velocity = new Vector2(RB2D.velocity.x/2,RB2D.velocity.y);
        //}
        //if (movementinput.y == 0)
        //{
        //    RB2D.velocity = new Vector2(RB2D.velocity.x, RB2D.velocity.y / 2);
        //}
    }

    public void OnMovementChange(InputAction.CallbackContext context)
    {
        movementinput = context.ReadValue<Vector2>();
    }

    public void OnCamChange(InputAction.CallbackContext context)
    {
        caminput = context.ReadValue<Vector2>();
    }

}
