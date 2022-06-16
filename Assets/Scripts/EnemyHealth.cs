using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxEnemyHealth = 100;
    int currentEnemyHealth;

    private void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
    }

    public void EnemyTakeDamage(int damage)
    {
        currentEnemyHealth -= damage;

        if (currentEnemyHealth <= 0)
            Destroy(gameObject);
    }
}
