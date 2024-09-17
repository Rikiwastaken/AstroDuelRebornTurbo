using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoiner : MonoBehaviour
{

    

    public InputActionAsset inputActions;

    public GameObject PlayerPrefab;

    int LastID=1;

    bool activated;

    public AnimatorController[] animators;


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
                    int rd = UnityEngine.Random.Range(0, animators.Length);
                    obj.GetComponent<Animator>().runtimeAnimatorController = animators[rd];
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
        Application.targetFrameRate = 60;

        if (activated)
        {
            activated = false;
        }
    }


}
