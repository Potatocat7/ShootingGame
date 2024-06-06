using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//エディターで呼び出せるように
[CreateAssetMenu(fileName = "EnemyEntity", menuName = "Create EnemyEntity")]
public class EnemyEntity : ScriptableObject
{
    public Sprite image;
    public int scorePoint;
    public BulletSelector.BulletTypeKind typeBullet;
    public int bulletInterval;
    public float enemySpeed;
    public int enemyHitPoint;
    public EnemyManager.EnemyMoveTypeKind moveType;
    public EnemyManager.EnBullDirTypeKind bulletDirection;
}
