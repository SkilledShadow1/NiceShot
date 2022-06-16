using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletLife = 5f;
    [SerializeField] LayerMask[] ignoreLayers;
    EnemyHealth enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < ignoreLayers.Length; i++) 
        {
            if (collision.gameObject.tag == "Enemy") 
            {
                enemy = collision.gameObject.GetComponent<EnemyHealth>();
                enemy.EnemyTakeDamage(10);
                Destroy(gameObject);
            }


            if (collision.gameObject.layer == 0)
            {
                Destroy(gameObject);
                return;
            }
              
                
        }


        
    }

    private void Start()
    {
        Destroy(gameObject, bulletLife);
    }
}
