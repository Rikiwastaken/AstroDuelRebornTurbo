using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boundscript : MonoBehaviour
{

    public int ID;//0=north/south, 1=east/west


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BigBomb>() == null)
        {
            Displacement(collision);
        }
        else if(!collision.GetComponent<BigBomb>().isexplosion)
        {
            Displacement(collision);
        }
        

    }


    private void Displacement(Collider2D collision)
    {
        if (ID == 0)
        {
            collision.transform.position = new Vector2(collision.transform.position.x, -collision.transform.position.y) - new Vector2(0, -collision.transform.position.y / Mathf.Abs(collision.transform.position.y) * 0.2f);
        }
        if (ID == 1)
        {
            collision.transform.position = new Vector2(-collision.transform.position.x, collision.transform.position.y) - new Vector2(-collision.transform.position.x / Mathf.Abs(collision.transform.position.x) * 0.2f, 0f); ;
        }

        if (collision.GetComponent<projectilescript>() != null)
        {
            if (collision.GetComponent<projectilescript>().hitbounds <= 2)
            {
                collision.GetComponent<projectilescript>().hitbounds++;
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
