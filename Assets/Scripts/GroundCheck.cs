using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool playerIsGrounded;

    public List<string> nonGroundObjects = new List<string>();

    [SerializeField] GameObject player;

    private Shotgun shotgun;

    private void Start()
    {
        shotgun = player.GetComponentInChildren<Shotgun>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < nonGroundObjects.Count; i++) 
        {

            if (collision.gameObject.tag == nonGroundObjects[i])
            {
                return;
            }
        }

        playerIsGrounded = true;
        shotgun.Reload();
        Debug.Log("AAA");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        for(int i = 0; i < nonGroundObjects.Count; i++) 
        { 
            if(collision.gameObject.tag == nonGroundObjects[i]) 
            {
                return;
            }
        }

        playerIsGrounded = false;
    }
}
