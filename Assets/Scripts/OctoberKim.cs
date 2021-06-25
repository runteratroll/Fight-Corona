using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctoberKim : EnemyMove
{
    public AudioClip[] audioClipsC;
    private AudioSource CannonSource;  
    public Transform firePosition_Enemy; // - ÃÑ±¸À§Ä¡
    public GameObject bulletPackageFactory; // - ÃÑ¾Ë°øÀå
    GameObject bulletPackage;
    GameObject childD;
    private void Awake()
    {
        childD = transform.GetChild(2).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        target = new Vector2(7, transform.position.y);
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        CannonSource = GetComponent<AudioSource>();

        hp = 3;
        score = 20;
    }

    private void Start()
    {
   
        StartCoroutine(Shoot());
        StartCoroutine(MoveEnemyObject());
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
            rigid.velocity = new Vector2(0, dirY);
        }
    }


    IEnumerator Shoot()
    {
        
        while (true)
        {
            yield return new WaitForSeconds(1.5f);

            CannonSource.Play();
            bulletPackage = Instantiate(bulletPackageFactory);

            bulletPackage.transform.position = firePosition_Enemy.position;
            Transform[] childs = bulletPackage.GetComponentsInChildren<Transform>(); //ÃÑ¾Ë ¹­À½¿¡¼­ ÀÚ½Ä 3°³¸¦ °¡Á®¿È

            for (int i = 0; i < childs.Length; i++) //°¢ ÃÑ¾ËÀÇ ºÎ¸ð-ÀÚ½Ä °ü°è¸¦ ²÷¾îÁÜ
            {
                childs[i].parent = null;

            }

            yield return new WaitForSeconds(5f);
            
        }
    }

    protected override IEnumerator Dead()
    {
        //CannonSource.clip = audioClipsC[1];
        //CannonSource.Play();
        childD.GetComponent<AudioSource>().Play();
        spriteRenderer.material.SetColor("_Color", Color.gray);
        col.enabled = false;

        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }




}
