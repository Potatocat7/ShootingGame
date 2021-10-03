using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InputGraphDataJson
{
    public EnemyStatus[] enemyStateList;
} 

[Serializable]
public struct EnemyStatus
{
    public int scorePoint;
    public BulletSelector.BulletTypeKind typeBullet;
    public int bulletInterval;
    public float enemySpeed;
    public int enemyHitPoint;
    public EnemyManager.EnemyMoveTypeKind moveType;
    public EnemyManager.EnBullDirTypeKind bulletDirection;
}

public class EnemyManager : MonoBehaviour
{
    public enum EnemyMoveTypeKind
    {
        Straight = 0,
        Wave,
        Stop,
        BossStraight,
        BossAround,
        BossReflection
    }
    public enum EnBullDirTypeKind
    {
        Straight = 0,
        TargetPlayer,
        SideShoot
    }
    [SerializeField]
    private EnemyCtrl _enemyPrefabObject;
    [SerializeField]
    private GameManger _gameManagerObject;
    [SerializeField]
    private BreakEffect _breakEffectPrefabObject;
    [SerializeField]
    private Enemy_Selector _enemySelectorObject;

     public int enemyRankUp { get; set; }

    private List<EnemyCtrl> _enemyObject = new List<EnemyCtrl>();
    private EnemyCtrl _enemyListObject;
    private BreakEffect _breakEffectObject;
    private Vector3 _breEfPosPlusY;
    private Vector3[] _enemyFirstPosition = new Vector3[5];
    private int _enemyCount;// = 5;
    private float _loopCntEnemyMake;
    private float _enemySpawnCount = 500;
    private int _enemyStageUp;
    private int _enemyPotisionChange;
    private int _enemyStartPotisionCnt;
    private bool _bossFlg;
    private bool _gameClearFlg;

    private InputGraphDataJson _graphJson;
    public void SpawnBreakEffect(Vector3 pos)//エフェクトの生成
    {
        _breEfPosPlusY = pos;
        _breEfPosPlusY.z -= 5;
        _breEfPosPlusY.y += 10;
        _breakEffectObject = Instantiate(_breakEffectPrefabObject, _breEfPosPlusY, Quaternion.Euler(-90,0,0));
    }
    public void AddEnemyScore(int point, EnemyMoveTypeKind move)//スコアへの加算
    {
        if (enemyRankUp == 6)
        {
            if (move >= EnemyMoveTypeKind.BossStraight)//BOSSステージ中の雑魚撃破でカウントされるのを回避
            {
                _gameManagerObject.ChangeRankUp(enemyRankUp + 1, _enemyStageUp + 1);
                if (_enemyStageUp == 2)
                {
                    _gameClearFlg = true;
                }
            }
        }
        else
        {
            _gameManagerObject.ChangeRankUp(enemyRankUp + 1, _enemyStageUp + 1);
        }
        _gameManagerObject.AddScoreScore(point, _gameClearFlg);
    }
    /*// 敵を生成する(JSONファイル経由)
    void CreateEnemy(int number, Vector3 newEnemyFirstPos, EnemyStatus State)
    {
        _enemyListObject = Instantiate(_enemyPrefabObject, newEnemyFirstPos, Quaternion.identity);
        _enemyListObject.parentObject = this;
        _enemyListObject.enemyListnum = number;

        _enemyListObject.enemyHitPoint = State.enemyHitPoint;
        _enemyListObject.thisSettingTypeBullet = State.typeBullet;
        _enemyListObject.enemyState = State;

        _enemyObject.Add(_enemyListObject);
    }*/

    // 敵を生成する(プレハブ経由)
    void CreateEnemy(int number, Vector3 newEnemyFirstPos, EnemyCtrl enemyPrefab)
    {
        _enemyListObject = Instantiate(enemyPrefab, newEnemyFirstPos, Quaternion.identity);//Stateに合わせたプレハブを指定
        _enemyListObject.parentObject = this;
        _enemyListObject.enemyListnum = number;

        //ここはプレハブで設定
        //_enemyListObject.enemyHitPoint = State.enemyHitPoint;
        //_enemyListObject.thisSettingTypeBullet = State.typeBullet;
        //_enemyListObject.enemyState = State;

        _enemyObject.Add(_enemyListObject);
    }

    // Start is called before the first frame update
    public void Init()
    {
        enemyRankUp = 0;
        _enemyStageUp = 0;
        _loopCntEnemyMake = 0;
        _enemyStartPotisionCnt = 0;
        _bossFlg = false;
        _gameClearFlg = false;
        //後で初期位置を参照してセットできるようにする？
        //_enemyFirstPosition[0] = new Vector3(0, 0, 30);
        //_enemyFirstPosition[1] = new Vector3(10, 0, 30);
        //_enemyFirstPosition[2] = new Vector3(-10, 0, 30);
        //_enemyFirstPosition[3] = new Vector3(20, 0, 50);
        //_enemyFirstPosition[4] = new Vector3(-20, 0, 50);

        //Jsonファイル読込
        string inputString = Resources.Load<TextAsset>("data").ToString();

        _graphJson = JsonUtility.FromJson<InputGraphDataJson>(inputString);
        _enemyPotisionChange = 1;
        _enemyCount = 0;
        StageUpEnemyDataChange(_enemyStageUp);
        //_enemyCount = 5;
        //for (int i = 0; i < _enemyCount; i++)
        //{
        //    CreateEnemy(i, _enemyFirstPosition[i], _graphJson.enemyStateList[0]);
        //}
    }
    public void RemoveAtObject(EnemyCtrl listNum)
    {
        _enemyObject.Remove(listNum);
        _enemyCount -= 1;
    }
    //public void ChangeEnenmyLevel()
    //{
    //    _enemyRankUp += 1;
    //}
    private void StageUpEnemyDataChange(int stage)
    {
        _enemySelectorObject.enemyPrefab_1.enemyState = _graphJson.enemyStateList[0 + 7 * stage];
        _enemySelectorObject.enemyPrefab_2.enemyState = _graphJson.enemyStateList[1 + 7 * stage];
        _enemySelectorObject.enemyPrefab_3.enemyState = _graphJson.enemyStateList[2 + 7 * stage];
        _enemySelectorObject.enemyPrefab_4.enemyState = _graphJson.enemyStateList[3 + 7 * stage];
        _enemySelectorObject.enemyPrefab_5.enemyState = _graphJson.enemyStateList[4 + 7 * stage];
        _enemySelectorObject.enemyPrefab_6.enemyState = _graphJson.enemyStateList[5 + 7 * stage];
        _enemySelectorObject.enemyPrefab_7.enemyState = _graphJson.enemyStateList[6 + 7 * stage];
    }
    void CreateEnemyUpdate()
    {
        _loopCntEnemyMake += 200.0f * Time.deltaTime;
        if (_loopCntEnemyMake >= _enemySpawnCount)
        {
            switch (enemyRankUp)
            {
                case 0:
                    CreateEnemy(_enemyCount, new Vector3(0, 0, 60), _enemySelectorObject.enemyPrefab_1);
                    _enemyCount += 1;
                    break;
                case 1:
                    switch (_enemyStageUp % 3)
                    {
                        case 0:
                            CreateEnemy(_enemyCount, new Vector3(0, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            break;
                        case 1:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            break;
                        case 2:
                            CreateEnemy(_enemyCount, new Vector3(0, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_2);
                            _enemyCount += 1;
                            break;
                    }
                    _enemyPotisionChange = 0;
                    break;
                case 2:
                    if (_enemyStageUp < 1)
                    {
                        CreateEnemy(_enemyCount, new Vector3(20 * _enemyPotisionChange, 0, 60), _enemySelectorObject.enemyPrefab_3);
                        _enemyCount += 1;
                        _enemyStartPotisionCnt += 1;
                        switch (_enemyStartPotisionCnt % 3)
                        {
                            case 0:
                                _enemyPotisionChange = 0;
                                break;
                            case 1:
                                _enemyPotisionChange = 1;
                                break;
                            case 2:
                                _enemyPotisionChange = -1;
                                break;
                        }
                    }
                    else
                    {
                        CreateEnemy(_enemyCount, new Vector3(0, 0, 60), _enemySelectorObject.enemyPrefab_3);
                        _enemyCount += 1;
                        CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_3);
                        _enemyCount += 1;
                        CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_3);
                        _enemyCount += 1;
                    }
                    break;
                case 3:
                    switch (_enemyStageUp % 3)
                    {
                        case 0:
                            if (_enemyPotisionChange == 0) { _enemyPotisionChange = 1; }
                            CreateEnemy(_enemyCount, new Vector3(-20 * _enemyPotisionChange, 0, 60), _enemySelectorObject.enemyPrefab_4);
                            _enemyCount += 1;
                            _enemyPotisionChange *= -1;
                            break;
                        case 1:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_4);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_4);
                            _enemyCount += 1;
                            break;
                        case 2:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_4);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_4);
                            _enemyCount += 1;
                            break;
                    }
                    break;
                case 4:
                    switch (_enemyStageUp % 3)
                    {
                        case 0:
                            CreateEnemy(_enemyCount, new Vector3(-20 * _enemyPotisionChange, 0, 60), _enemySelectorObject.enemyPrefab_5);
                            _enemyCount += 1;
                            _enemyPotisionChange *= -1;
                            break;
                        case 1:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_5);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_5);
                            _enemyCount += 1;
                            break;
                        case 2:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_5);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_5);
                            _enemyCount += 1;
                            break;
                    }
                    break;
                case 5:
                    switch (_enemyStageUp % 3)
                    {
                        case 0:
                            CreateEnemy(_enemyCount, new Vector3(-20 * _enemyPotisionChange, 0, 60), _enemySelectorObject.enemyPrefab_6);
                            _enemyCount += 1;
                            _enemyPotisionChange *= -1;
                            break;
                        case 1:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_6);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_6);
                            _enemyCount += 1;
                            break;
                        case 2:
                            CreateEnemy(_enemyCount, new Vector3(20, 0, 60), _enemySelectorObject.enemyPrefab_6);
                            _enemyCount += 1;
                            CreateEnemy(_enemyCount, new Vector3(-20, 0, 60), _enemySelectorObject.enemyPrefab_6);
                            _enemyCount += 1;
                            break;
                    }
                    _bossFlg = false;
                    break;
                case 6:
                    if (_bossFlg == false)
                    {
                        CreateEnemy(_enemyCount, new Vector3(0, 0, 60), _enemySelectorObject.enemyPrefab_7);
                        _enemyCount += 1;
                        _bossFlg = true;
                    }
                    break;
                default:
                    break;

            }
            _loopCntEnemyMake = 0;
            if (enemyRankUp > 6)
            {
                if (_enemyStageUp < 2)
                {
                    _enemyStageUp += 1;
                    if (_enemyStageUp == 3)
                    {
                        _gameClearFlg = true;
                    }
                    else
                    {
                        StageUpEnemyDataChange(_enemyStageUp);
                    }
                    enemyRankUp = 0;
                }
                else
                {
                }
            }
        }

    }
    void BulletMakeUpdate(Vector3 pos)
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            if (_enemyObject[i])
            {
                _enemyObject[i].MoveAction(_enemyObject[i].enemyState.moveType);
                if (_enemyObject[i].enemyState.typeBullet != BulletSelector.BulletTypeKind.None)
                {
                    _enemyObject[i].SetEnemyBulletDirection(pos, _enemyObject[i].enemyState.bulletDirection);
                    _enemyObject[i].bulletCount += 100.0f * Time.deltaTime;
                    if (_enemyObject[i].bulletCount >= _enemyObject[i].enemyState.bulletInterval)
                    {
                        _enemyObject[i].MakeBullet(GameManger.MachineTypeKind.Enemy, 
                                                   _enemyObject[i].GetEnemyBulletDirection().normalized, 
                                                   _enemyObject[i].enemyState.typeBullet);
                        _enemyObject[i].bulletCount = 0;
                    }
                }
            }
        }

    }
    // Update is called once per frame
    public void ChildUpdate(Vector3 pos)
    {
        //敵機の新規生成
        CreateEnemyUpdate();

        //弾の射出処理
        BulletMakeUpdate(pos);
    }
}
