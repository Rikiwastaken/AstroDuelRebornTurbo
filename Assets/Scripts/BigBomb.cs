using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BigBomb : MonoBehaviour
{
    public int timebeforedetonation;

    private Tilemap tilemap;

    public bool isexplosion;

    public int explosionduration;

    private bool launchedexplosion;

    public float explosiondurationinsec;

    private GameObject parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.tag == "decor")
        {

            if (tilemap == null)
            { tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>(); }

            Vector3 hitPosition = Vector3.zero;
            if (tilemap != null && tilemap.gameObject == collision.gameObject)
            {
                Vector3 newpoint = collision.ClosestPoint(transform.position);
                tilemap.SetTile(tilemap.WorldToCell(newpoint), null);
            }

        }

        if (collision.transform.tag == "projectile")
        {
            Destroy(collision.transform.gameObject);
        }

        if(collision.transform.tag =="Player")
        {
            Destroy(collision.transform.gameObject);
        }

    }

    private void FixedUpdate()
    {
        

        if(isexplosion)
        {
            if(explosionduration > 0)
            {
                explosionduration--;
                transform.localScale = new Vector3(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f, 1f);
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
                transform.GetChild(0).GetComponent<BigBomb>().explosionduration = (int)(explosiondurationinsec / Time.deltaTime);
                transform.GetChild(0).GetComponent<BigBomb>().parent = this.gameObject;



            }
        }
    }

}
