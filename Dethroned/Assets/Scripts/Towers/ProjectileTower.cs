using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : TowerBase
{
    [Header("Projectiles")]
    public float TowerDamage = 5;
    public float TowerAttackSpeed = 1;
    public float TowerProjectileSpeed = 2;

    public GameObject ProjectilePrefab;
    private List<GameObject> _projectiles = new List<GameObject>();

    private IEnumerator attackEnemies()
    {
        while (true)
        {
            var wait = new WaitForSeconds(TowerAttackSpeed + Random.Range(-0.1f,0.1f));

            // if there is a target to attack.
            if (_target)
            {
                // Damage the Enemies


                // Create a projectile aimed at the enemy and shoot it
                var proj = fetchProjectile();
                proj.transform.position = transform.position; // reset position

                // reset any values needed and prep for use.
                if (!proj.activeInHierarchy)
                {
                    proj.SetActive(true);
                    if (proj.TryGetComponent<Rigidbody2D>(out var __projR))
                    {
                        __projR.velocity = Vector2.zero; // reset velocity
                    }
                    if(proj.TryGetComponent<Projectile>(out var __projP))
                    {
                        __projP.target = null;
                    }
                }

                // use the projectile
                if (proj.TryGetComponent<Rigidbody2D>(out var _projR))
                {
                    var dir = (_target.transform.position - transform.position).normalized;
                    proj.transform.right = dir;
                    _projR.AddForce(dir * TowerProjectileSpeed);
                }
                if(proj.TryGetComponent<Projectile>(out var _projP))
                {
                    _projP.target = _target;
                    _projP.EnemyLayer = EnemyLayer;
                }


                // play sounds


                // etc...
            }

            yield return wait;
        }
    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(attackEnemies());
    }

    private GameObject fetchProjectile()
    {
        GameObject obj = _projectiles.Find(p => !p.activeInHierarchy);
        if (!obj) { obj = Instantiate(ProjectilePrefab, transform); _projectiles.Add(obj); }
        return obj;
    }

    private void OnValidate()
    {
        if (!ProjectilePrefab) Debug.LogError($"{name} is missing a Projectile Prefab.");
    }
}
