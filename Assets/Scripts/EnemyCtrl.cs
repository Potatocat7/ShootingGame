using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : Machine
{
    public EnemyManager parentObject { get; set; }
    /*//JSON用
    public int enemyListnum { get; set; }
    public int enemyHitPoint { get; set; }
    public float bulletCount { get; set; }
    public EnemyStatus enemyState { get; set; }
    */
    public int enemyListnum { get; set; }
    //public int enemyHitPoint;
    public float bulletCount;
    public EnemyStatus enemyState;

    private Vector3 _enemyBulletDirection;
    private float _enemyMoveAround = 0;
    private bool _changeMoveDire = false;
    private bool _changeMoveDire_z = false;
    //ランダム移動用
    private Vector3 savePos = new Vector3();
    private Vector3 randamTarget = new Vector3();
    private int cnt = 0;
    /// <summary>画像</summary>
    [SerializeField] private SpriteRenderer spriteRenderer;
    /// <summary>モデル</summary>
    private EnemyModel enemyModel;

    public void SetEnemyBulletDirection(Vector3 pos , EnemyManager.EnBullDirTypeKind dirType)
    {
        switch (dirType)
        {
            case EnemyManager.EnBullDirTypeKind.Straight:
                _enemyBulletDirection =  Vector3.back;
                break;
            case EnemyManager.EnBullDirTypeKind.TargetPlayer:
                _enemyBulletDirection = (pos - this.transform.position);
                break;
            case EnemyManager.EnBullDirTypeKind.SideShoot:
                if ((pos - this.transform.position).x > 0)
                {
                    _enemyBulletDirection = Vector3.right;
                }
                else
                {
                    _enemyBulletDirection = Vector3.left;
                }
                break;
        }
    }
    public void Init(int num)
    {
        enemyModel = new EnemyModel(num);
        spriteRenderer.sprite = enemyModel.image;
        enemyState.scorePoint = enemyModel.scorePoint;
        enemyState.typeBullet = enemyModel.typeBullet;
        enemyState.bulletInterval = enemyModel.bulletInterval;
        enemyState.enemySpeed = enemyModel.enemySpeed;
        enemyState.enemyHitPoint = enemyModel.enemyHitPoint;
        enemyState.moveType = enemyModel.moveType;
        enemyState.bulletDirection = enemyModel.bulletDirection;
    }
    public Vector3 GetEnemyBulletDirection()
    {
        return _enemyBulletDirection;
    }

    public void MoveAction(EnemyManager.EnemyMoveTypeKind moveType)
    {
        Vector3 changePos = this.transform.position;
        switch (moveType)
        {
            case EnemyManager.EnemyMoveTypeKind.Straight:
                changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                break;
            case EnemyManager.EnemyMoveTypeKind.Wave:
                if (changePos.x > 30.0f)
                {
                    _changeMoveDire = true;
                }
                else if (changePos.x < -30.0f)
                {
                    _changeMoveDire = false;
                }
                if (_changeMoveDire == false)
                {
                    changePos.x += enemyState.enemySpeed * Time.deltaTime;
                }
                else
                {
                    changePos.x -= enemyState.enemySpeed * Time.deltaTime;
                }
                changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                break;
            case EnemyManager.EnemyMoveTypeKind.Stop:
                if (changePos.z >= 0)
                {
                    changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                }
                break;
            case EnemyManager.EnemyMoveTypeKind.BossStraight:
                if (changePos.z > 55.0f)
                {
                    _changeMoveDire = true;
                }
                else if (changePos.z < -55.0f)
                {
                    _changeMoveDire = false;
                }
                if (_changeMoveDire == false)
                {
                    changePos.z += enemyState.enemySpeed * Time.deltaTime;
                }
                else
                {
                    changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                }
                break;
            case EnemyManager.EnemyMoveTypeKind.BossAround:
                switch (_enemyMoveAround % 4)
                {
                    case 0:
                        changePos.x += enemyState.enemySpeed * Time.deltaTime;
                        if (changePos.x > 30.0f)
                        {
                            _enemyMoveAround += 1;
                        }
                        break;
                    case 1:
                        changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                        if (changePos.z < -55.0f)
                        {
                            _enemyMoveAround += 1;
                        }
                        break;
                    case 2:
                        changePos.x -= enemyState.enemySpeed * Time.deltaTime;
                        if (changePos.x < -30.0f)
                        {
                            _enemyMoveAround += 1;
                        }
                        break;
                    case 3:
                        changePos.z += enemyState.enemySpeed * Time.deltaTime;
                        if (changePos.z > 55.0f)
                        {
                            _enemyMoveAround += 1;
                        }
                        break;
                }
                break;
            case EnemyManager.EnemyMoveTypeKind.BossReflection:
                if (changePos.x > 30.0f)
                {
                    _changeMoveDire = true;
                }
                else if (changePos.x < -30.0f)
                {
                    _changeMoveDire = false;
                }
                if (changePos.z > 55.0f)
                {
                    _changeMoveDire_z = true;
                }
                else if (changePos.z < -55.0f)
                {
                    _changeMoveDire_z = false;
                }
                if (_changeMoveDire == false)
                {
                    changePos.x += enemyState.enemySpeed * Time.deltaTime;
                }
                else
                {
                    changePos.x -= enemyState.enemySpeed * Time.deltaTime;
                }
                if (_changeMoveDire_z == false)
                {
                    changePos.z += enemyState.enemySpeed * Time.deltaTime;
                }
                else
                {
                    changePos.z -= enemyState.enemySpeed * Time.deltaTime;
                }
                break;
            case EnemyManager.EnemyMoveTypeKind.WaveRandam:
                //var pos = transform.position; changePos
                cnt += 1;
                if (cnt >=800)
                {
                    cnt = 0;
                    randamTarget.x = UnityEngine.Random.Range(-30.0f, 30.0f);
                    randamTarget.z = UnityEngine.Random.Range(-55.0f, 55.0f);
                    randamTarget.y = 0.0f;
                }
                savePos += (randamTarget - changePos) * enemyState.enemySpeed * 0.2f * Time.deltaTime;
                savePos -= savePos * enemyState.enemySpeed * 0.05f * Time.deltaTime;
                changePos += savePos * Time.deltaTime;

                break;
        }
        this.transform.position = changePos;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            if (other.gameObject.tag == "Finish")
            {
                if (base.onlyOnceHitFlg == false) 
                {
                    base.onlyOnceHitFlg = true;
                    parentObject.RemoveAtObject(this);
                    this.OnhitDamge();
                }
            }
            else
            {
                if (other.gameObject.tag == "Enemy")
                {

                }
                else if (other.gameObject.GetComponent<Bullet>().bulletMachineType == GameManger.MachineTypeKind.Player)
                {
                    if (base.onlyOnceHitFlg == false)
                    {
                        enemyState.enemyHitPoint -= 1;
                        if (enemyState.enemyHitPoint <= 0)
                        {
                            parentObject.AddEnemyScore(enemyState.scorePoint, enemyState.moveType);
                            base.onlyOnceHitFlg = true;
                            parentObject.RemoveAtObject(this);
                            parentObject.SpawnBreakEffect(this.transform.position);
                            this.OnhitDamge();
                        }
                    }
                    other.GetComponent<Bullet>().Onhit();
                }
            }
        }
    }
}
