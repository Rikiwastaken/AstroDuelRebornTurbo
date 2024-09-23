using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusScript : MonoBehaviour
{

    public int hp;

    private bool showlife;

    public GameObject[] bonuslist;

    public int cooldown;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag=="Player")
        {

            int rd = Random.Range(0, bonuslist.Length);
            collision.GetComponent<movement>().heldbonus=bonuslist[rd];



            cooldown = (int)(30/Time.deltaTime);

        }
        if(collision.transform.tag=="projectile")
        {
            showlife = true;
            Destroy(collision.gameObject);

            hp--;
            if(hp <= 0)
            {
                cooldown = (int)(30 / Time.deltaTime);
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

        if(cooldown > 0)
        {
            showlife=false;
            transform.GetChild(0).gameObject.SetActive(false);
            hp = 3;
            cooldown--;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }

    }

}
