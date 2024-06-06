using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public Sprite image;
    public int scorePoint;
    public BulletSelector.BulletTypeKind typeBullet;
    public int bulletInterval;
    public float enemySpeed;
    public int enemyHitPoint;
    public EnemyManager.EnemyMoveTypeKind moveType;
    public EnemyManager.EnBullDirTypeKind bulletDirection;

    public EnemyModel(int num)
    {
        EnemyEntity enemyEntity = Resources.Load<EnemyEntity>("Enemy/Enemy_" + num.ToString());
        image = enemyEntity.image;
        scorePoint = enemyEntity.scorePoint;
        typeBullet = enemyEntity.typeBullet;
        bulletInterval = enemyEntity.bulletInterval;
        enemySpeed = enemyEntity.enemySpeed;
        enemyHitPoint = enemyEntity.enemyHitPoint;
        moveType = enemyEntity.moveType;
        bulletDirection = enemyEntity.bulletDirection;
    }
}
