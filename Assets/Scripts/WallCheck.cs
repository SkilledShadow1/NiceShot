using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    public string[] nonWallTags;
    public bool isTouchingWall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < nonWallTags.Length; i++) 
        {
            if (collision.gameObject.tag == nonWallTags[i]) 
            {
                return;
            }
        }

        print("az");
        isTouchingWall = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTouchingWall = false;
    }
}
