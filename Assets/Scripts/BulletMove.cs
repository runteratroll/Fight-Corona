using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public enum Type { Player, Enemy, Laser};
    public Type type;
    //[SerializeField]
    protected float speed = 10f;
    GameManager gameManager = null;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        CheckLimit();
        if (type == Type.Player)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            
        }
        if (type == Type.Enemy)
        {
            transform.localScale = new Vector2(0.5f, 0.5f);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }

    private void CheckLimit()
    {
        if (transform.position.y > gameManager.MaxPosition.y + 10f)
        {
            Despawn();
        }
        if (transform.position.y < gameManager.MinPosition.y - 2f)
        {
            Despawn();
        }
        if (transform.position.x > gameManager.MaxPosition.x + 2f)
        {
            Despawn();
        }
        if (transform.position.x < gameManager.MinPosition.x - 2f)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        if (type == Type.Enemy) return;
        transform.SetParent(gameManager.poolManager.transform, false);
        gameObject.SetActive(false);
    }
}

