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

    public int immobilizationframes;

    private Vector2 lastmovementinput;

    public float dashlength;

    public int dashtime;


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

        

        if(heldbonus != null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if(heldbonus.GetComponent<SpriteRenderer>())
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = heldbonus.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = heldbonus.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
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

        if ( dashtime == 0)
        {
            if (caminput != Vector2.zero)
            {
                lastmovementinput = caminput;

                float angleA = 0f;
                if (Mathf.Atan2(caminput.x, caminput.y) * Mathf.Rad2Deg != 0)
                {
                    angleA = Mathf.Atan2(caminput.x, -caminput.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
                }

                if (caminput == new Vector2(0f, 1f))
                {
                    transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }

                Dashfct(caminput);
                Gunfct(caminput);
                UseSpecial(caminput);

            }
            else if (movementinput != Vector2.zero)
            {

                lastmovementinput = movementinput;

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
                UseSpecial(movementinput);
            }
            else
            {
                float angleA = 0f;
                if (Mathf.Atan2(lastmovementinput.x, lastmovementinput.y) * Mathf.Rad2Deg != 0)
                {
                    angleA = Mathf.Atan2(lastmovementinput.x, -lastmovementinput.y) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
                }

                if (lastmovementinput == new Vector2(0f, 1f))
                {
                    transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }


                Dashfct(lastmovementinput);
                Gunfct(lastmovementinput);
                UseSpecial(lastmovementinput);
            }
        }

        

        averagespeed = (float)(Mathf.Abs(RB2D.velocity.x) + Mathf.Abs(RB2D.velocity.y))/2f;

        if(movementinput != null && averagespeed < maxspeed && dashtime == 0)
        {
            RB2D.AddForce(movementinput*speedvect);
        }

        if(immobilizationframes > 0)
        {
            immobilizationframes--;
            RB2D.velocity=Vector2.zero;
            Color color = new Color(0.6f, 0.6f, 0.6f, 1f);
            GetComponent<SpriteRenderer>().color = color;
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Color color = new Color(1f, 1f, 1f, 1f);
            GetComponent<SpriteRenderer>().color = color;
            transform.GetChild(2).gameObject.SetActive(false);
        }

        if (dashtime > 0)
        {
            dashtime--;
        }


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
        if (dashinput && !dashed && dashtime==0)
        {
            RB2D.velocity=Vector2.zero;
            dashed = true;
            RB2D.AddForce(dir * dashstr);
            dashtime = (int)(dashlength / Time.deltaTime);
        }
    }

    public void Gunfct(Vector2 dir)
    {
        if (guninput && !guned && dashtime == 0)
        {
            GameObject newproj = Instantiate(projectileprefab,this.transform.position, Quaternion.identity);
            guned = true;
            newproj.GetComponent<projectilescript>().sender = this.gameObject;
            newproj.GetComponent<projectilescript>().direction = dir;
            newproj.GetComponent<projectilescript>().timebeforeselfharm = (int)(timebeforeselfharm*Time.deltaTime);
        }
    }

    void UseSpecial(Vector2 direction)
    {
        if (specialinput && heldbonus != null && dashtime == 0)
        {
            if (heldbonus.GetComponent<BigBomb>() != null)
            {
                GameObject newbomb = Instantiate(heldbonus, this.transform.position+(new Vector3(direction.x, direction.y,0f)) / 5f, Quaternion.identity);
                newbomb.GetComponent<BigBomb>().timebeforedetonation = (int)(180);

            }

            if (heldbonus.GetComponent<MagBomb>() != null)
            {
                GameObject newbomb = Instantiate(heldbonus, this.transform.position + (new Vector3(direction.x, direction.y, 0f)) / 5f, Quaternion.identity);
                newbomb.GetComponent<MagBomb>().timebeforedetonation = (int)(60);
                newbomb.GetComponent<MagBomb>().sender = transform.gameObject;
            }

            if (heldbonus.GetComponent<Wallbonusscript>() != null)
            {
                GameObject newwall = Instantiate(heldbonus, this.transform.position + (new Vector3(direction.x, direction.y, 0f)), Quaternion.identity);

                float angleA = 0f;
                if (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg != 0)
                {
                    angleA = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
                    newwall.transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
                }

                if (direction == new Vector2(0f, 1f))
                {
                    newwall.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }

            }

            if(heldbonus.GetComponent<Pusherscript>() != null)
            {
                GameObject newpusher = Instantiate(heldbonus, this.transform.position + (new Vector3(direction.x, direction.y, 0f)), Quaternion.identity);

                direction = -direction;

                float angleA = 0f;
                if (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg != 0)
                {
                    angleA = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
                    newpusher.transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
                }

                if (direction == new Vector2(0f, 1f))
                {
                    newpusher.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }

                newpusher.GetComponent<Pusherscript>().direction = direction;
            }

            if (heldbonus.GetComponent<megalaserbonus>() != null)
            {
                Vector3 position = this.transform.position + (new Vector3(direction.x, direction.y, 0f) / 6);
                GameObject newlaser = Instantiate(heldbonus, position, Quaternion.identity);
                

                direction = -direction;

                float angleA = 0f;
                if (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg != 0)
                {
                    angleA = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
                    newlaser.transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
                }

                if (direction == new Vector2(0f, 1f))
                {
                    newlaser.transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }

                newlaser.GetComponent<megalaserbonus>().duration = 20;
                immobilizationframes = newlaser.GetComponent<megalaserbonus>().duration;
                newlaser.GetComponent<megalaserbonus>().sender = gameObject;
                newlaser.GetComponent<megalaserbonus>().position = position;
                newlaser.transform.position = position;
            }


            heldbonus = null;
        }

        
    }

}
