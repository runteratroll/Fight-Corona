using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBimmmm : BulletMove
{

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet")
            Destroy(collision.gameObject);
            //gameObject.SetActive(false);
    }
}
