using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 10f;

    public float health = 100;

    public int value = 50;

    public GameObject deathEffect;
    
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerStats.Money += value;
        
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect,5f);
        
        Destroy(gameObject);
    }
}
