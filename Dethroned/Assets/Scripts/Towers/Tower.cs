using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Provides the Basic functionality of the Towers of the game. allows for reusablility of code for expansive tower types.
/// </summary>
public abstract class Tower : MonoBehaviour
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

    /// <summary>
    /// Looks for enemies in a radius around the tower and updates a private global variable with the list of enemies found.
    /// </summary>
    protected void _detectEnemies()
    {
        // look for enemies in a circle around the tower and update list of detected enemies
        _detectedEnemies = Physics2D.OverlapCircleAll(transform.position, radius, EnemyLayer).ToList();
        // sorts by distance from tower elements at index 0 are closer than elements at index n
        _detectedEnemies = _detectedEnemies.OrderBy(x => Vector2.Distance(x.transform.position, transform.position)).ToList();
    }

    
    private IEnumerator __randomUpdate()
    {
        // we don't want to have a lump sum of towers attempting to do certain operations all at the same time
        // so use a random number to give different timings to each tower.
        var wait = new WaitForSeconds(Random.Range(.25f, .75f));
        while (true)
        {
            // call random update for things that don't need to be done every frame but do need to happen on a consistent basis.
            _randomUpdate();
            yield return wait;
        }
    }

    /// <summary>
    /// A function that is called randomly in a range of .25 -> .75 seconds.
    /// basic implementation does enemy detection.
    /// </summary>
    protected virtual void _randomUpdate()
    {
        _detectEnemies();
    }


    [Header("Targeting")]
    public TargetingPriority targetPrio;
    public enum TargetingPriority { CLOSE, FAR, CASTLE, WEAK, STRONG }

    protected GameObject _castle;

    protected virtual void Start()
    {
        _health = MaxHealth;
        _castle = GameObject.FindGameObjectWithTag("Player");
        if (!_castle) Debug.LogError($"{name} couldn't find castle (looking for gameobject with tag \"Player\")");
        
    }

    protected virtual void startAllCoroutines()
    {
        StartCoroutine(__randomUpdate());
    }

    protected virtual void OnEnable()
    {
        startAllCoroutines();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
