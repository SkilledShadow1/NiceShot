using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    GameObject player;
    PlayerMovement2 playerCode;
    bool canHit;
    float invincibiltyTimer = 0;
    float invincibilityTotalTime = 2f;
    [SerializeField] int damage;


    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            player = collision.gameObject;
            playerCode = player.GetComponent<PlayerMovement2>();
            if (invincibiltyTimer >= invincibilityTotalTime) 
            {
                invincibiltyTimer = 0;
                playerCode.Damage(damage);
            }
                
            
        }
    }

    private void Start()
    {
        invincibiltyTimer = invincibilityTotalTime;
    }

    private void Update()
    {
        if (invincibiltyTimer < invincibilityTotalTime)
            invincibiltyTimer += Time.deltaTime;
    }

}
