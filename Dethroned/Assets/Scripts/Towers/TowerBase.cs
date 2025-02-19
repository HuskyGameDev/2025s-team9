using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Provides the Basic functionality of the Towers of the game. allows for reusablility of code for expansive tower types.
/// </summary>
public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Costs")]
    public float CostOfTower;

    [Header("Tower Health")]
    public float MaxHealth = 100;
    protected float _health;
    public void DamageHealth(float amt)
    {
        // Damage the tower
        _health = Mathf.Clamp(_health - amt, 0, MaxHealth);

        // any special code can go after this point for example sounds, debug, particles, etc...
        Debug.Log($"{gameObject.name} took {amt} damage.");
    }
    /// <summary>
    /// Restores the health of the tower UPTO its max health
    /// </summary>
    /// <param name="amt">amount to heal the tower by.</param>
    public void RestoreHealth(float amt)
    {
        // heal the tower.
        _health = Mathf.Clamp(_health + amt, 0, MaxHealth);

        // any special code can go after this point for example sounds, debug, particles, etc...
        Debug.Log($"{gameObject.name} healed for {amt}.");
    }

    [Header("Enemy Detection")]
    public float radius = 5;
    public LayerMask EnemyLayer;
    protected List<Collider2D> _detectedEnemies = new List<Collider2D>();
    protected GameObject _target;

    /// <summary>
    /// Looks for enemies in a radius around the tower and updates a private global variable with the list of enemies found.
    /// </summary>
    void detectEnemies()
    {
        // look for enemies in a circle around the tower and update list of detected enemies
        _detectedEnemies = Physics2D.OverlapCircleAll(transform.position, radius, EnemyLayer).ToList();
        // sorts by distance from tower elements at index 0 are closer than elements at index n
        _detectedEnemies = _detectedEnemies.OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).ToList();

        if (_detectedEnemies.Count != 0)
            switch (targetPrio)
            {

                case TargetingPriority.CLOSE:
                    _target = _detectedEnemies[0].gameObject;
                    break;

                case TargetingPriority.FAR:
                    _target = _detectedEnemies[_detectedEnemies.Count - 1].gameObject;
                    break;

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
                    _target = closesetToCastle;
                    break;

                case TargetingPriority.WEAK:
                    Debug.LogWarning($"Update detectEnemies() to target Weak enemy types");
                    break;

                case TargetingPriority.STRONG:
                    Debug.LogWarning($"Update detectEnemies() to target Strong enemy types");
                    break;
            }
    }
    private IEnumerator randomUpdate()
    {
        // we don't want to have a lump sum of towers attempting to do certain operations all at the same time
        // so use a random number to give different timings to each tower.
        var wait = new WaitForSeconds(Random.Range(.5f, 1.5f));
        while (true)
        {
            detectEnemies();
            yield return wait;
        }
    }

    [Header("Targeting")]
    public TargetingPriority targetPrio;
    public enum TargetingPriority { CLOSE, FAR, CASTLE, WEAK, STRONG }

    private GameObject _castle;

    protected virtual void Start()
    {
        _castle = GameObject.FindGameObjectWithTag("Player");
        if (!_castle) Debug.LogError($"{name} couldn't find castle (looking for gameobject with tag \"Player\")");
        StartCoroutine(randomUpdate());
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
