using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private GameObject __target;
    private Vector3 __targetStart;
    public Vector2 offset, bounds;
    private LayerMask __enemyLayer;

    private float __speed;
    [HideInInspector] public bool _penetration;

    [HideInInspector] public UnityEvent<Projectile,GameObject> enemyHit;

    private AudioSource __audioSource;
    private Rigidbody2D __rb;

    public void Init()
    {
        __target = null;
        __rb.velocity = Vector2.zero;
    }

    public void Init(GameObject tar, LayerMask EL, float speed, bool penetration)
    {
        // update enemy targeting.
        __target = tar;
        __targetStart = __target.transform.position;
        __enemyLayer = EL;
        _penetration = penetration;

        // position the projectile
        __speed = speed;
        var dir = (tar.transform.position - transform.position).normalized;
        transform.right = dir;
    }

    private void Awake()
    {
        __audioSource = GetComponent<AudioSource>();
        __rb = GetComponent<Rigidbody2D>();
    }

    public void PlayAudio(AudioClip clip)
    {
        __audioSource.PlayOneShot(clip);
    }

    private void Update()
    {

        if (__target && !_penetration) // if its not a penetrating projectile then track the target.
        {
            var dir = (__target.transform.position - transform.position).normalized;
            transform.right = dir;
            transform.position+=(dir*__speed*Time.deltaTime);
        }else if (_penetration) // if it is a penetrating projectile then shoot towards the first position you saw the target at.
        {
            var dir = (__targetStart - transform.position).normalized;
            transform.right = dir;
            transform.position += (dir * __speed * Time.deltaTime);
            if(Vector3.Distance(transform.position,__targetStart) < 0.1f)
            {
                // since the final target was never hit the listener from a tower is still active on this projectile so lets just clear all listening parties and disable this projectile.
                enemyHit.RemoveAllListeners();
                gameObject.SetActive(false);
            }
        }else
        {
            gameObject.SetActive(false);
            return;
        }
        
    }

    List<Collider2D> prevCollisions = new List<Collider2D>(); // if we are a penetrating projectile then we need to keep track of previous collisions so we don't accidentally do more damage than intended to enemies.
    private void FixedUpdate()
    {
        var collision = Physics2D.OverlapBoxAll(transform.position + (Vector3)offset, bounds, 0, __enemyLayer);
        foreach(var collider in collision)
        {
            if (collider.gameObject.Equals(__target)) // hit target
            {
                enemyHit.Invoke(this, __target);
                prevCollisions = new List<Collider2D>();
                gameObject.SetActive(false);
            }
            else if (!prevCollisions.Contains(collider) && _penetration) enemyHit.Invoke(this, collider.gameObject); // we hit an enemy just not THE target and we are a penetrating projectile.
            prevCollisions.Add(collider);
        }
            
    }

    private void OnValidate()
    {
        if ((bounds.x == 0 && bounds.y == 0) && (transform.lossyScale.x != 0 && transform.lossyScale.y != 0)) bounds = transform.lossyScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, (Vector3)bounds);
    }
}
