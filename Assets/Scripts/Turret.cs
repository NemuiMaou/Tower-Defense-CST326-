using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;
    
    [Header("General")]
    
    public float range = 15f;
    
    [Header("Use Bullets (default)")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [Header("Use Laser")] 
    public bool useLaser = false;

    public int damageOverTime = 30;
    public float slowPct = .5f;
    
    public LineRenderer linerenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;
    
    [Header("Unity Setup Fields")]
    
    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;
    
    public Transform firePoint;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (useLaser)
            {
                if (linerenderer.enabled)
                {
                    linerenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            
            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        // Target lockon
        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        
        if (!linerenderer.enabled)
        {
            linerenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        linerenderer.SetPosition(0, firePoint.position);
        linerenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;
        
        impactEffect.transform.position = target.position + dir.normalized;
        
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);

    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        //Debug.Log("pew");
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
