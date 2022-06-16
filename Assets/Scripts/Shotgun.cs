using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    //Imported Stuff
    [SerializeField] Rigidbody2D playerRigidbody;

    [SerializeField] float shotgunForce;

    [SerializeField] GameObject player;

    [SerializeField] CooldownScriptableObject shotgunCooldown;

    [SerializeField] float shotTimer = 1f;

    private CooldownManager cm;

    private CooldownData shotgunFireRate;

    private CooldownData frictionStopper;

    public int bulletNumber;

    public Rigidbody2D bullet;

    public float bulletSpeed;

    public Transform bulletSpawn;

    [SerializeField] int currentBullets;

    [SerializeField] int maxBullets;

    GroundCheck gc;

    private void Start()
    {
        cm = CooldownManager.Instance;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        shotgunFireRate = cm.FindCooldown(CooldownData.CooldownType.SHOTGUN);
        frictionStopper = cm.FindCooldown(CooldownData.CooldownType.FRICTION);
        gc = player.GetComponent<GroundCheck>();
        currentBullets = maxBullets;

    }

    void Update()
    {
        faceMouse();
        Shoot();
        //Debug.Log($"IsReady: {data.IsReady()} | Time: {data.timer} | Max: {data.requiredTime}");
    }



    private void faceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2
        (
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        transform.right = direction;
    }


    private void Shoot()
    {
        if (PlayerInput.Instance.pressingShoot && shotgunFireRate.IsReady() && currentBullets > 0)
        {
            currentBullets--;
            frictionStopper.timer = 0;
            shotgunFireRate.timer = 0;

            playerRigidbody.velocity = new Vector2(
            -(gameObject.transform.right.x * shotgunForce) + playerRigidbody.velocity.x,
            -(gameObject.transform.right.y * shotgunForce) +
            playerRigidbody.velocity.y
            );

            Bullets();
        }


    }

    public void Reload() 
    {
            currentBullets = maxBullets;
    }

    public float xSpread;

    public float ySpread;

    private void Bullets()
    {
        List<Rigidbody2D> bullets = new List<Rigidbody2D>();


        for (int i = 0; i < bulletNumber; i++)
        {
            bullets.Add((Rigidbody2D)Instantiate(bullet, bulletSpawn.position, this.transform.rotation));
            Vector3 direction = Quaternion.Euler(0, UnityEngine.Random.Range(-xSpread, xSpread), UnityEngine.Random.Range(-ySpread, ySpread)) * transform.right;
            bullets[i].velocity = direction * bulletSpeed;

        }

    }


}
