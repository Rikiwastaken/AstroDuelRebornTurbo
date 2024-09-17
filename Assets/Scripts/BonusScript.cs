using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScript : MonoBehaviour
{

    public int hp;

    private bool showlife;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Player")
        {
            Debug.Log("bonus !");
            Destroy(this.gameObject);

        }
        if(collision.transform.tag=="projectile")
        {
            showlife = true;
            Destroy(collision.gameObject);

            hp--;
            if(hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if(showlife)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponentInChildren<Slider>().value = hp;
        }
    }

}
