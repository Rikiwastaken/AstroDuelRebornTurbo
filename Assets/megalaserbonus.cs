using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class megalaserbonus : MonoBehaviour
{
    public int duration;

    private Tilemap tilemap;

    public GameObject sender;

    public Vector3 position;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "decor")
        {

            if (tilemap == null)
            { tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>(); }

            Vector3 hitPosition = Vector3.zero;
            if (tilemap != null && tilemap.gameObject == collision.gameObject)
            {
                ContactPoint2D[] contacts = collision.contacts;
                foreach (ContactPoint2D hit in contacts)
                {

                    Vector3 newpoint = hit.point;
                    tilemap.SetTile(tilemap.WorldToCell(newpoint), null);
                }
            }

        }

        if (collision.transform.tag == "projectile" || collision.transform.tag == "wallbonus" || (collision.transform.GetComponent<Pusherscript>()!=null && collision.transform.parent==null))
        {
            Destroy(collision.transform.gameObject);
        }

        if (collision.transform.tag == "Player" && collision.transform.gameObject!=sender)
        {
            Destroy(collision.transform.gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position = position;
        if(duration > 0)
        {
            duration--;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
