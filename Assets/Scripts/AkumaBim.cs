using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkumaBim : EnemyMove
{
    public AudioClip[] audioClipsB;
    private AudioSource AkumaLaserSource;
    

    [SerializeField]
    private GameObject bulletPre = null;
    [SerializeField]
    private Transform bulletPos = null;
    GameObject bulletA;
    GameObject childDead;

    private void Start()
    {
        childDead = transform.GetChild(1).gameObject;
        AkumaLaserSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
        gameObject.layer = 0;
        target = new Vector2(7, transform.position.y);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //AkumaLaserSource = GetComponent<AudioSource>();
        //gameManager = FindObjectOfType<GameManager>();
        col = GetComponent<Collider2D>();
        StartCoroutine(Fire());
        hp = 2;
        score = 15;
    }

    private void Update()
    {
        if (transform.position.x >= 7)
            transform.position = Vector3.MoveTowards(transform.position, target, 5f * Time.deltaTime);
    }

    //private GameObject InstantiateOrPool()
    //{
    //    GameObject result = null;
    //    // 자식이 있을 경우(재활용 가능)
    //    if (gameManager.poolManager.transform.childCount > 0)
    //    {
    //        result = gameManager.poolManager.transform.GetChild(0).gameObject;
    //        result.transform.position = bulletPos.position;
    //        result.transform.SetParent(null);
    //        result.SetActive(true);
    //    }
    //    // 자식이 없을 경우(새로 생성)
    //    else
    //    {
    //        GameObject newBullet = Instantiate(bulletPre, bulletPos);
    //        newBullet.transform.position = bulletPos.position;
    //        newBullet.transform.SetParent(null);
    //        result = newBullet;
    //    }

    //    return result;
    //}
    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(3f);
        
        while (true)
        {
            AkumaLaserSource.Play();
            bulletA = Instantiate(bulletPre, bulletPos);
            bulletA.transform.SetParent(null);

            yield return new WaitForSeconds(3f);
            Destroy(bulletA);
            yield return new WaitForSeconds(3f);

        }
    }


    protected override IEnumerator Dead()
    {
        //AkumaLaserSource.clip = audioClipsB[0];
        //AkumaLaserSource.Play();
        childDead.GetComponent<AudioSource>().Play();
        spriteRenderer.material.SetColor("_Color", Color.gray);
        col.enabled = false;
        Destroy(gameObject);
        yield return new WaitForSeconds(1f);
        
        Destroy(bulletA);
    }


}
