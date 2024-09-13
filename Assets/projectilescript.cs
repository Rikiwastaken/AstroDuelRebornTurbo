using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class projectilescript : MonoBehaviour
{

    public GameObject sender;

    public Vector2 direction;

    public float speed;

    private Tilemap tilemap;

    public float hitcolincr;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject != sender)
        {

            if(collision.gameObject.tag=="cadre")
            {
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag == "decor")
            {

                Vector3 hitPosition = Vector3.zero;
                if (tilemap != null && tilemap.gameObject == collision.gameObject)
                {
                    ContactPoint2D[] contacts = collision.contacts;
                    foreach (ContactPoint2D hit in contacts)
                    {

                        Vector3 newpoint = hit.point;
                        tilemap.SetTile(tilemap.WorldToCell(newpoint),null);
                        
                    }
                }


                Destroy(this.gameObject);
            }

        }
    }

    private void FixedUpdate()
    {

        if (tilemap == null)
        { tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>(); }

        GetComponent<Rigidbody2D>().velocity = direction*speed;

        

        float angleA = 0f;
        if (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg != 0)
        {
            angleA = Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, 0f, angleA + 90);
        }

        if (direction == new Vector2(0f, 1f))
        {
            transform.eulerAngles = new Vector3(0f, 0f, -90f);
        }


        Vector2 vel = GetComponent<Rigidbody2D>().velocity;

    }
}
