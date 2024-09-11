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

    public InputActionReference dash;

    private Vector2 movementinput;

    private Vector2 caminput;

    private bool dashinput;

    public float maxspeed;

    public float speedvect;

    private Rigidbody2D RB2D;

    private float averagespeed;

    public float dashstr;

    private bool dashed;

    


    // Start is called before the first frame update
    void Start()
    {
        move.action.performed += OnMovementChange;

        cam.action.performed += OnCamChange;

        dash.action.performed += OnDashChange;

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

        if (!dash.action.IsPressed())
        {
            dashinput = false;
            dashed = false;
        }

        

        if (caminput!=Vector2.zero)
        {
            float angleA = 0f;
            if (Mathf.Atan2(caminput.x, caminput.y) * Mathf.Rad2Deg != 0)
            { 
                angleA = Mathf.Atan2(caminput.x, -caminput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }
            Dashfct(caminput);

        }
        else if(movementinput!=Vector2.zero)
        {
            float angleA = 0f;
            if (Mathf.Atan2(movementinput.x, movementinput.y) * Mathf.Rad2Deg != 0)
            {
                angleA = Mathf.Atan2(movementinput.x, -movementinput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }
            Dashfct(movementinput);
        }
        else
        {
            Dashfct(new Vector2(-transform.right.x,transform.right.y));
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

    public void OnDashChange(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>()!=0f)
        {
            dashinput = true;
        }
        else
        {
            dashinput = false;
        }
    }

    public void Dashfct(Vector2 dir)
    {
        if (dashinput && !dashed)
        {
            RB2D.velocity=Vector2.zero;
            dashed = true;
            RB2D.AddForce(dir * dashstr);
        }
    }

}
