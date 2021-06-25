using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMove : MonoBehaviour
{
    GameManager gameManager;
    public float speed;

    
    private int value;
    void Start()
    {
        
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
 

        if(gameManager.ranPoint == 0)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if(gameManager.ranPoint == 1)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }


    }
}
