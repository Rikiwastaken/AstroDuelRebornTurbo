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

    public int playerID;

    public InputActionAsset inputActions;

    private InputAction move;

    private InputAction cam;

    private InputAction dash;

    private InputAction gun;

    private InputAction special;

    private Vector2 movementinput;

    private Vector2 caminput;

    private bool specialinput;

    private bool dashinput;

    private bool guninput;

    public float maxspeed;

    public float speedvect;

    private Rigidbody2D RB2D;

    private float averagespeed;

    public float dashstr;

    private bool dashed;

    private bool guned;

    public GameObject projectileprefab;

    public string playername;

    private bool initialize = true;

    public float timebeforeselfharm;

    public GameObject heldbonus;


    // Start is called before the first frame update
    void Initialize()
    {
        initialize = false;
        GameObject[] playerlist = GameObject.FindGameObjectsWithTag(this.tag);

        int ID = 1;

        foreach (GameObject player in playerlist)
        {
            if(player!=this.gameObject)
            {
                ID++;
            }
        }

        playername = "player"+ID;

        move = inputActions.FindActionMap(playername).FindAction("move");

        cam = inputActions.FindActionMap(playername).FindAction("camera");

        gun = inputActions.FindActionMap(playername).FindAction("gun");

        dash = inputActions.FindActionMap(playername).FindAction("dash");

        special = inputActions.FindActionMap(playername).FindAction("special");

        move.performed += OnMovementChange;

        cam.performed += OnCamChange;

        dash.performed += OnDashChange;

        gun.performed += OnGunChange;

        special.performed += OnSpecialChange;

        RB2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if(initialize)
        {
            Initialize();
        }

        RB2D.angularVelocity = 0;

        if (!move.IsPressed())
        {
            movementinput = Vector2.zero;
            
        }

        if (!cam.IsPressed())
        {
            caminput = Vector2.zero;
        }

        if (!dash.IsPressed())
        {
            dashinput = false;
            dashed = false;
        }

        if (!gun.IsPressed())
        {
            guninput = false;
            guned = false;
        }

        if (!special.IsPressed())
        {
            specialinput = false;
        }

        if(specialinput && heldbonus!=null)
        {
            UseSpecial();
        }



        if (caminput!=Vector2.zero)
        {
            float angleA = 0f;
            if (Mathf.Atan2(caminput.x, caminput.y) * Mathf.Rad2Deg != 0)
            { 
                angleA = Mathf.Atan2(caminput.x, -caminput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }

            if(caminput == new Vector2(0f,1f))
            {
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
            }

            Dashfct(caminput);
            Gunfct(caminput);

        }
        else if(movementinput!=Vector2.zero)
        {
            float angleA = 0f;
            if (Mathf.Atan2(movementinput.x, movementinput.y) * Mathf.Rad2Deg != 0)
            {
                angleA = Mathf.Atan2(movementinput.x, -movementinput.y) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
            }

            if (movementinput == new Vector2(0f, 1f))
            {
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
            }

            Dashfct(movementinput);
            Gunfct(movementinput);
        }
        else
        {
            Dashfct(new Vector2(-transform.right.x,transform.right.y));
            Gunfct(new Vector2(-transform.right.x, transform.right.y));
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
        if(playerID!=1)
        {
            Debug.Log("player2 moved");
        }
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

    public void OnGunChange(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() != 0f)
        {
            guninput = true;
        }
        else
        {
            guninput = false;
        }
    }

    public void OnSpecialChange(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() != 0f)
        {
            specialinput = true;
        }
        else
        {
            specialinput = false;
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

    public void Gunfct(Vector2 dir)
    {
        if (guninput && !guned)
        {
            GameObject newproj = Instantiate(projectileprefab,this.transform.position, Quaternion.identity);
            guned = true;
            newproj.GetComponent<projectilescript>().sender = this.gameObject;
            newproj.GetComponent<projectilescript>().direction = dir;
            newproj.GetComponent<projectilescript>().timebeforeselfharm = (int)(timebeforeselfharm*Time.deltaTime);
        }
    }

    void UseSpecial()
    {

        if (heldbonus.GetComponent<BigBomb>() != null)
        {
            GameObject newbomb = Instantiate(heldbonus,this.transform.position, Quaternion.identity);
            newbomb.GetComponent<BigBomb>().timebeforedetonation = (int)(180);

        }

        heldbonus = null;
    }

}
