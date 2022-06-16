using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    public bool enemyIsGrounded;

    public List<string> nonGroundObjects = new List<string>();

    [SerializeField] GameObject enemy;

    private float groundTime = 0f;
    [SerializeField] float maxGroundTime = 0f;
    public bool canJump;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < nonGroundObjects.Count; i++)
        {

            if (collision.gameObject.tag == nonGroundObjects[i])
            {
                return;
            }
        }

        enemyIsGrounded = true;
        Debug.Log("!!!");
    }

    private void StayGrounded() 
    {
        if (enemyIsGrounded == false)
            groundTime = 0;

        if (groundTime > maxGroundTime)
        {
            print("why");
            canJump = true;
            return;
        }
        else
            canJump = false;

        if (enemyIsGrounded == true)
            groundTime += Time.deltaTime;   
        
    }

    private void Update()
    {
        StayGrounded();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        for (int i = 0; i < nonGroundObjects.Count; i++)
        {
            if (collision.gameObject.tag == nonGroundObjects[i])
            {
                return;
            }
        }

        enemyIsGrounded = false;
        groundTime = 0;
    }
}
