using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTower : Tower
{

    [Header("Projectile ~Art~")]
    [Tooltip("the projectile that the tower is shooting\n\n[NOTE: USE A UNIQUE NAME ON THE PREFAB IF YOU WANT SPECIFIC PROJECTILES TO BE TIED TO SPECIFIC TOWERS THE WAY IT DETERMINES WHICH PROJECTILE TO USE IS BASED ON THE NAME OF THE PROJECTILE ITSELF.]")]
    public GameObject ProjectilePrefab;
    private static List<GameObject> _projectiles = new List<GameObject>();
    public List<AudioClip> TowerProjectileSounds = new List<AudioClip>();

    [Header("Projectile ~Stats~")]
    public float TowerDamage = 5;
    public float TowerAttackSpeed = 1;
    public float TowerProjectileSpeed = 2;
    public bool penetration = false;


    protected GameObject _target;
    /// <summary>
    /// Basic target finding for single target towers.
    /// </summary>
    /// <returns>the target for the tower (NULL if no enemies found)</returns>
    protected virtual GameObject _findTarget()
    {
        if (_detectedEnemies.Count != 0)
            switch (targetPrio)
            {

                case TargetingPriority.CLOSE:
                    return _detectedEnemies[0].gameObject;

                case TargetingPriority.FAR:
                    return _detectedEnemies[_detectedEnemies.Count - 1].gameObject;

                case TargetingPriority.CASTLE:
                    GameObject closesetToCastle = _detectedEnemies[0].gameObject;
                    float closestDist = Vector2.Distance(closesetToCastle.transform.position, _castle.transform.position);
                    foreach (Collider2D e in _detectedEnemies)
                    {
                        var dist = Vector2.Distance(e.transform.position, _castle.transform.position);
                        if (closestDist > dist)
                        {
                            closestDist = dist;
                            closesetToCastle = e.gameObject;
                        }
                    }
                    return closesetToCastle;

                case TargetingPriority.WEAK:
                    
                    GameObject weakest = _detectedEnemies[0].gameObject;
                    foreach (Collider2D e in _detectedEnemies)
                    {
                        if (e.TryGetComponent<EnemyStats>(out var stat))
                        {
                            e.TryGetComponent<EnemyStats>(out var tstat);
                            if (stat.enemyMaxHealth < tstat.enemyMaxHealth)
                                weakest = stat.gameObject;
                        }
                    }
                    return weakest;

                case TargetingPriority.STRONG:
                    GameObject strong = _detectedEnemies[0].gameObject;
                    foreach (Collider2D e in _detectedEnemies)
                    {
                        if (e.TryGetComponent<EnemyStats>(out var stat))
                        {
                            e.TryGetComponent<EnemyStats>(out var tstat);
                            if (stat.enemyMaxHealth < tstat.enemyMaxHealth)
                                strong = stat.gameObject;
                        }
                    }
                    return strong;
            }
        return null;
    }

    protected override void _randomUpdate()
    {
        base._randomUpdate();
        _target = _findTarget();
    }

    protected void _attackEnemy()
    {
        // if there is a target to attack.
        if (_target)
        {
            // Create a projectile aimed at the enemy and shoot it
            var proj = __launchProjectile();

            // Damage the Enemy after the projectile lands a hit.
            proj.enemyHit.AddListener(_damageEnemy);

            // etc...

        }
    }

    /// <summary>
    /// whenever a projectile spawned from the tower hits an enemy it will attempt to call this function. and upon hitting the destination target will remove the listener from the projectile.
    /// </summary>
    /// <param name="proj">projectile that is causing the damage</param>
    /// <param name="enemy">the enemy hit by the projectile</param>
    protected virtual void _damageEnemy(Projectile proj, GameObject enemy)
    {
        if (enemy.Equals(_target))
            proj.enemyHit.RemoveListener(_damageEnemy); // we don't need to listen anymore since we hit the target and this projectile could potentially be reused by another tower.

        if (penetration)
        {
            var hitsRes = Physics2D.LinecastAll(transform.position, _target.transform.position);
            foreach (var hit in hitsRes)
            {
                var hitObj = hit.collider.gameObject;
                if (hitObj.Equals(enemy))
                {
                    __damageActual(hitObj);
                }
            }
            return;
        }

        __damageActual(_target);

        return;
    }

    /// <summary>
    /// will do the call to the gameobject and do the damage to the gameobject.
    /// </summary>
    /// <param name="Enemy">target that is going to be damaged.</param>
    protected virtual void __damageActual(GameObject Enemy)
    {
        if(Enemy) Enemy.GetComponent<EnemyStats>()?.DamageEnemy(TowerDamage);
    }

    private IEnumerator __attackEvent()
    {
        while (true)
        {
            var wait = new WaitForSeconds(TowerAttackSpeed + Random.Range(-0.1f, 0.1f));
            yield return wait;

            _attackEnemy();

        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void startAllCoroutines()
    {
        base.startAllCoroutines();
        StartCoroutine(__attackEvent());
    }

    private GameObject __fetchProjectile()
    {
        //GameObject obj = _projectiles.Find(p => !p.activeInHierarchy && p.name.Equals(ProjectilePrefab.name + "(Clone)"));
        //if (!obj) { obj = Instantiate(ProjectilePrefab); _projectiles.Add(obj); }
        return DethronedUtility.FetchPooledGameObject(_projectiles,ProjectilePrefab);
    }
    private Projectile __launchProjectile()
    {
        return __launchProjectile(_target);
    }
    private Projectile __launchProjectile(GameObject tar)
    {
        var proj = __fetchProjectile();
        proj.transform.position = transform.position; // reset position
        proj.TryGetComponent<Projectile>(out var projP);

        // reset any values needed and prep for use.
        if (!proj.activeInHierarchy)
        {
            proj.SetActive(true);
        }

        // use the projectile
        projP?.Init(tar, EnemyLayer, TowerProjectileSpeed, penetration);
        // play the sound on the projectile.
        int index = Random.Range(0, TowerProjectileSounds.Count);
        projP?.PlayAudio(TowerProjectileSounds[index]);

        return projP;
    }

    private void OnValidate()
    {
        if (!ProjectilePrefab) Debug.LogError($"{name} is missing a Projectile Prefab.");
    }
}
