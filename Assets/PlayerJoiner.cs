using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{

    public InputActionAsset inputActions;

    public GameObject PlayerPrefab;

    int LastID=1;

    bool activated;


    public void JoinPlayer()
    {
        if(!activated)
        {
            activated = true;
            

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");


            foreach (GameObject obj in objects)
            {
                if (obj.GetComponent<movement>().playerID == 0)
                {
                    //obj.GetComponent<movement>().inputActions = inputActions;

                    obj.GetComponent<movement>().playerID = LastID;
                }
            }

            LastID += 1;
        }

        

        

    }

    public void PlayerLeave()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");

        List<int> playerIDs = new List<int>();

        foreach (GameObject obj in objects)
        {
            playerIDs.Add(obj.GetComponent<movement>().playerID);
        }

        int tempid = 1;

        while(playerIDs.Contains(tempid))
        {
            tempid++;
        }

        foreach (GameObject obj in objects)
        {
            if(obj.GetComponent<movement>().playerID==tempid)
            {
                Destroy(obj);
            }
        }

    }

    private void FixedUpdate()
    {
        if(activated)
        {
            activated = false;
        }
    }


}
