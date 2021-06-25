using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float skillTime = 0f;
    float maxTime = 15f;
    bool isSwingReady;
    float eSkillTime = 0f;
    float maxEskillTime = 1f;
    bool eDown;
    public bool laserDuring = false;
    public GameObject LaserPrefab = null;
    public Transform LaserPoint = null;
    [SerializeField]
    private AudioSource[] shootingSource;
    [SerializeField]
    private VirtualController virtualController;


    public BoxCollider2D meleeArea; //무기 범위 
    public TrailRenderer trailEffect; //효과


    public float moveSpeed = 3f;
    [SerializeField]
    private float bulletDelay = 0.1f;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    Transform bulletPosition;
    SpriteRenderer spriteRenderer = null; 
    bool isDamaged = false;
    public bool qDown = false;
    public bool isFireReady;
    GameManager gameManager;
    public ObjectManager objectManager;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Fire());
       
    }
    void Update()
    {
        SkillTime();
        Move();
        InputManager();
    }

    void InputManager()
    {
        qDown = Input.GetButton("SpeacialSkill");
        eDown = Input.GetButtonDown("Swing");

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "EnemyBullet" && gameObject.layer == 10)
        {

            Debug.Log("닿");
            other.gameObject.tag = "reflectBullet";
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 15f, ForceMode2D.Impulse);
            
            
        }
        
        if (other.tag == "EnemyBullet")
        {
           
            Debug.Log("닿았다.");
            if (isDamaged) return;
            isDamaged = true;
            StartCoroutine(Damage());
        }
        
    }
    private void Move()
    {
        float x = virtualController.Horizontal(); //left / Right
        float y = virtualController.Vertical(); // Up / Down
     

        transform.position += new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8.7f, 8.7f), Mathf.Clamp(transform.position.y, -4.2f, 4.2f));

    }

    private IEnumerator Fire()
    {

        

        while(true)
        {
            if (gameObject.layer == 8)
            {
                yield return new WaitForSeconds(4.5f);
                gameObject.layer = 0;
            }
            shootingSource[0].Play();
            InstantiateOrPool();
  
            yield return new WaitForSeconds(bulletDelay);
        }

      

    }

    private GameObject InstantiateOrPool()
    {
        GameObject result = null;
        // 자식이 있을 경우(재활용 가능)
        if (gameManager.poolManager.transform.childCount > 0)
        {
            result = gameManager.poolManager.transform.GetChild(0).gameObject;
            result.transform.position = bulletPosition.position;
            result.transform.SetParent(null);
            result.SetActive(true);
        }
        // 자식이 없을 경우(새로 생성)
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletPosition);
            newBullet.transform.position = bulletPosition.position;
            newBullet.transform.SetParent(null);
            result = newBullet;
        }

        return result;
    }

    private IEnumerator Damage()
    {
        gameManager.Dead();
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;
    }

    private IEnumerator SpeacialLaser() //Coroutin 함수는 구현만 해놔야 겠다 (공격시간을 주거나 하는 건 힘들다)
    {
        laserDuring = true;
        moveSpeed = 7f;
        Debug.Log("발사");
        GameObject Laser = Instantiate(LaserPrefab, LaserPoint);
        gameObject.layer = 8;
        shootingSource[2].Play();
        yield return new WaitForSeconds(4.5f);
        shootingSource[2].Play();
        Destroy(Laser);
        qDown = false;
        laserDuring = false;
        moveSpeed = 3f;

    }

    private void SkillTime()  //스킬 함수를 만들어 관리편하게 
    {
        skillTime += 1f * Time.deltaTime; // 스킬에서만 시간을 더하고
        eSkillTime += 1f * Time.deltaTime;

        //Debug.Log("SkillTime : " + skillTime);
        isFireReady = ( maxTime < skillTime);
        isSwingReady = (maxEskillTime < eSkillTime);
    }

    public void OnclikEskill()
    {
        if (isSwingReady && !laserDuring) 
        {
            //스윙 만드는중
            anim.SetTrigger("Swing");

            StartCoroutine(Swing());

            eSkillTime = 0f;
        }
    }

    public void OnclikQskill()
    {
        if (isFireReady)
        {
            StartCoroutine(SpeacialLaser());

            skillTime = 0f;
        }
    }

    private IEnumerator Swing() //열거형 함수 클래스
    {
        gameObject.layer = 10;
        shootingSource[1].Play(); //Swing 소리

         yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        trailEffect.enabled = false;
        gameObject.layer = 0;
    }

}
