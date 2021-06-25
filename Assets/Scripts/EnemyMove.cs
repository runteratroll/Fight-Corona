using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    GameObject childS;
    protected long score = 10; 
    protected int hp = 1;
    protected float randomY;
    protected Vector2 target;
    public enum Type {Enemy, October, Akuma};
    public Type type;
    public float bulletDelay;
    [SerializeField]
    private GameObject bulletPrefab;
    GameObject result;
    public AudioClip[] audioClipsA;
    private AudioSource EnemyDeadS;
    protected Rigidbody2D rigid;
    private GameObject bullet;
    protected SpriteRenderer spriteRenderer = null;

    protected GameManager gameManager = null;
 
    protected Collider2D col = null;

    protected bool isDamage;
    protected bool isDead;
    void Start()
    {
        childS = transform.GetChild(0).gameObject;
        EnemyDeadS = GetComponent<AudioSource>();
        target = new Vector2(7, transform.position.y);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        col = GetComponent<Collider2D>();
        StartCoroutine(MoveEnemyObject());
        StartCoroutine(Fire());
    }


    private void Update()
    {

        if (transform.position.x >= 7)
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
    }

    private IEnumerator MoveEnemyObject()
    {

        float dirX;
        float dirY;
        while (true)
        {
            dirX = Random.Range(-0.7f, 0.5f);
            dirY = Random.Range(-0.5f, 0.5f);

            yield return new WaitForSeconds(1f);
            rigid.velocity = new Vector2(dirX, dirY);
        }
    }

    private IEnumerator Fire()
    {
        GameObject bullet;

        while (true)
        {
            //enemyShootS.Play();
            childS.GetComponent<AudioSource>().Play();
            //EnemyShootSource.clip = audioClipsA[0];
            //EnemyShootSource.Play();
            bullet = Instantiate(bulletPrefab, transform);
            bullet.transform.SetParent(null);
            yield return new WaitForSeconds(bulletDelay);
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "PlayerBullet" || other.tag == "LaserBim" || other.tag == "reflectBullet") //|| bullet.tag == "reflectBullet"
        {
            if (other.tag == "PalyerBullet")
                other.gameObject.SetActive(false);
               // Destroy(other.gameObject);

            if (other.tag == "LaserBim")
            {
                
                gameManager.AddScore(score);
                StartCoroutine(Dead());
            }
            if (hp > 0)
            {
                if (isDamage) return;

                isDamage = true;
                StartCoroutine(Damage());

                return;
            }
            if (isDead) return;
            isDead = true;
            gameManager.AddScore(score); // ctrl + r 하면 이름 바뀜  

            StartCoroutine(Dead());

        }
    }


 


    protected IEnumerator Damage()
    {
        hp--;
        spriteRenderer.material.color = Color.gray;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        isDamage = false;
    }

    protected virtual IEnumerator Dead()
    {
        EnemyDeadS.Play();
        spriteRenderer.material.SetColor("_Color", Color.gray);
        col.enabled = false;

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

