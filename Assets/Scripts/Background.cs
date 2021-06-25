using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector2 offset = Vector2.zero;
    MeshRenderer meshRenderer = null;
    [SerializeField]
    float speed = 3f;
    private float skillDuration = 4.5f;

    Player player = null;
    float timer = 0f;
    bool isSkillPlaying = false;

    void Start()
    {
        player = FindObjectOfType<Player>();
        meshRenderer = GetComponent<MeshRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (player.laserDuring && player.isFireReady) //q를 누르면 
        {
            isSkillPlaying = true; //조건을 하나 더 추가해 누를때가 아닌 누른후 해제로 변경
        }
        if(isSkillPlaying && timer < skillDuration) 
        {
            offset.x += 5f * Time.deltaTime;
            timer += 1f * Time.deltaTime;
          
        }
        else if(timer >= skillDuration)
        {
            isSkillPlaying = false;
            timer = 0f;

        }
        else
        {
            offset.x += speed * Time.deltaTime;
        }
        
        meshRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}
