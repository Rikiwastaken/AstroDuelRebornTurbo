using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MagBomb : MonoBehaviour
{
    public int timebeforedetonation;

    private Tilemap tilemap;

    public bool isexplosion;

    public int explosionduration;

    private bool launchedexplosion;

    public float explosiondurationinsec;

    private GameObject parent;

    public GameObject sender;

    private void OnTriggerStay2D(Collider2D collision)
    {

        if(isexplosion)
        {
            if (collision.transform.tag == "projectile" || collision.transform.tag == "wallbonus" || (collision.transform.GetComponent<Pusherscript>() != null && collision.transform.parent == null))
            {
                Destroy(collision.transform.gameObject);
            }

            //if (collision.transform.tag == "Player" && collision.gameObject!=sender)
            if (collision.transform.tag == "Player")
            {
                collision.transform.GetComponent<movement>().immobilizationframes = 120;
            }
        }

        
    }

    private void FixedUpdate()
    {

        if(transform.parent != null)
        {
            sender = transform.parent.GetComponent<MagBomb>().sender;
        }
        

        if(isexplosion)
        {
            if(explosionduration > 0)
            {
                explosionduration--;
                transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                Destroy(parent);
            }

        }
        else
        {
            if (timebeforedetonation > 0)
            {
                timebeforedetonation--;
            }
            else if (!isexplosion && !launchedexplosion)
            {
                launchedexplosion = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<MagBomb>().explosionduration = (int)(explosiondurationinsec / Time.deltaTime);
                transform.GetChild(0).GetComponent<MagBomb>().parent = this.gameObject;



            }
        }
    }

}
