using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoberKimBullet : BulletMove
{

    void Update()
    {
        
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
