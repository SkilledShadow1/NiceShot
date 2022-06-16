using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float rotateSpeed;
    [SerializeField] float viewRadius = 5f;
    [SerializeField] float offset = 90f;
    float timeViewingPlayer;
    [SerializeField] float timeNeededToShoot = 5f;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] Rigidbody2D enemyBullet;
    [SerializeField] GameObject enemyGun;
    [SerializeField] float enemyBulletSpeed;
    bool canFire;
    [SerializeField] GameObject parentObject;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    void Update()
    {
        Rotate();
        if(canFire) 
        {
            EnemyShoot();
            canFire = false;
        }

    }

    private void Rotate() 
    {
        if (Vector3.Distance(transform.position, player.position) <= viewRadius)
        {

            Vector2 dir = player.position - transform.position;
            dir.Normalize();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            enemyGun.transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

            canFire = CanFire();
        }

    }

    private bool CanFire()
    {
        timeViewingPlayer += Time.deltaTime;
        if (timeViewingPlayer >= timeNeededToShoot) 
        {
            timeViewingPlayer = 0;
            return true;
        }
        return false;
    }

    private void EnemyShoot() 
    {
        Rigidbody2D currentBullet = (Rigidbody2D) Instantiate(enemyBullet, bulletSpawn.position, enemyGun.transform.rotation);
        currentBullet.velocity = -enemyGun.transform.right * enemyBulletSpeed;
    }



}
