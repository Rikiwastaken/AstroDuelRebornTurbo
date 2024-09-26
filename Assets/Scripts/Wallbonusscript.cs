using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallbonusscript : MonoBehaviour
{

    public int hp;

    private bool showlife;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "projectile")
        {
            showlife = true;
            Destroy(collision.gameObject);

            hp--;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (showlife)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponentInChildren<Slider>().value = hp;
        }
    }
}
