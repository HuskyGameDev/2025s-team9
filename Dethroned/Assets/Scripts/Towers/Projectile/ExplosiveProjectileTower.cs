using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectileTower : ProjectileTower
{
    public float explosiveRadius = 2f;

    private GameObject __debugEnemy;
    protected override void _damageEnemy(Projectile proj, GameObject enemy)
    {
        base._damageEnemy(proj, enemy);
        if (enemy == _target)
        {
            __debugEnemy = enemy;
            var colRes = Physics2D.OverlapCircleAll(enemy.transform.position, explosiveRadius, EnemyLayer);
            foreach (var col in colRes)
            {
                __damageActual(col.gameObject);
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if(__debugEnemy)
            Gizmos.DrawWireSphere(__debugEnemy.transform.position, explosiveRadius);
    }
}
