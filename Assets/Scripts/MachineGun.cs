using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] Rigidbody2D bullet;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float machineGunForce;
    CooldownData machineGunFireRate;
    CooldownData frictionStopper;
    private CooldownManager cm;
    Rigidbody2D rb;
    [SerializeField] GameObject player;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        cm = CooldownManager.Instance;
        machineGunFireRate = cm.FindCooldown(CooldownData.CooldownType.MACHINEGUN);
        frictionStopper = cm.FindCooldown(CooldownData.CooldownType.FRICTION);
    }

    private void Update()
    {
        faceMouse();
        Shoot();
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
        if (PlayerInput.Instance.pressingShoot && machineGunFireRate.IsReady())
        {
            //if (rb.velocity.y > 1)
              //  rb.velocity = new Vector2(rb.velocity.x, 1);
            frictionStopper.timer = 0;
            machineGunFireRate.timer = 0;

            rb.velocity = new Vector2(
            -(gameObject.transform.right.x * machineGunForce) + rb.velocity.x,
            -(gameObject.transform.right.y * machineGunForce) + rb.velocity.y
            );

            Bullet();
        }


    }

    private void Bullet()
    {
        Rigidbody2D bulletPrefab = (Rigidbody2D) Instantiate(bullet, bulletSpawn.position, this.transform.rotation);
        bulletPrefab.velocity = transform.right * bulletSpeed;

    }
}
