using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dash : MonoBehaviour
{
    public int dashtime;

    private Tilemap tilemap;

    private void FixedUpdate()
    {
        if(dashtime > 0)
        {
            dashtime--;
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != transform.parent)
        {

            if (collision.transform.tag == "projectile")
            {
                Destroy(collision.transform.gameObject);
            }

            if (collision.transform.tag == "Player")
            {
                Destroy(collision.transform.gameObject);
            }

            if(collision.transform.GetComponentInChildren<Pusherscript>())
            {
                collision.transform.GetComponentInChildren<Pusherscript>().hp--;
            }
        }
    }

}
