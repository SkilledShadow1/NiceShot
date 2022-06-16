using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float bulletLife = 5f;
    [SerializeField] LayerMask[] ignoreLayers;
    [SerializeField] GameObject player;
    PlayerMovement2 playerCode;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < ignoreLayers.Length; i++)
        {
            if(collision.gameObject.tag == "Player")
            {
                playerCode.Damage(50);
                Destroy(gameObject);
            }


            if (collision.gameObject.layer == ignoreLayers[i])
                Destroy(gameObject);
        }

    }

    private void Start()
    {
        player = GameObject.Find("Player");
        playerCode = player.GetComponent<PlayerMovement2>();
        Destroy(gameObject, bulletLife);
    }
}
