using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectilescript : MonoBehaviour
{

    public GameObject sender;

    public Vector2 direction;

    public float speed;


    void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject.name);

        if(collision.gameObject != sender)
        {

            if(collision.gameObject.tag=="cadre")
            {
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag == "decor")
            {
                Destroy(this.gameObject);
            }

        }
    }

    private void FixedUpdate()
    {
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
