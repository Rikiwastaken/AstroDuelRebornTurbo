using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Pusherscript : MonoBehaviour
{
    public Vector2 direction;

    public float pushstr;

    public float hp;

    private bool showlife;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "projectile")
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(direction*pushstr);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "projectile")
        {
            showlife = true;
            Destroy(collision.gameObject);

            hp--;
        }
    }

    private void FixedUpdate()
    {
        if (transform.parent != null)
        {
            direction = transform.parent.GetComponent<Pusherscript>().direction;
            pushstr = transform.parent.GetComponent<Pusherscript>().pushstr;
        }
        else
        {
            if (showlife)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                GetComponentInChildren<Slider>().value = hp;
            }
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
