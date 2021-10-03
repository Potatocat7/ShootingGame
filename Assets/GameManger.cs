using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    [SerializeField]
    private PlayerCtrl _playerPrefabObject;
    [SerializeField]
    private EnemyManager _enemyMangeObject;
    //[SerializeField]
    //private RestManager _socketMangeObject;
    [SerializeField]
    private Text _ScoreText;
    [SerializeField]
    private Text _FailureText;
    [SerializeField]
    private Text _NextText;

    private float _mapTop = 57.0f;
    private float _mapUnder = -57.0f;
    private float _mapLeft = -32.0f;
    private float _mapRight = 32.0f;
    //private int _playerSpeed = 20;
    //private float _playerBulletCount = 0.0f;
    //private int _playerBulletInterval = 230;

#if UNITY_IPHONE || UNITY_ANDROID
    private int _playerSpeed = 20;
    private float _playerBulletCount = 0.0f;
    private int _playerBulletInterval = 230;
    private Vector2 _playerInputStartPosition;
    private Vector2 _playerInputMovePosition;
    private Vector2 _playerInputMoveNormalized;
#else
    private float _fUpMove = 30.0f;
    private float _fDownMove = -30.0f;
    private float _fLeftMove = -30.0f;
    private float _fRightMove = 30.0f;
#endif
    public enum MachineTypeKind
    {
        None = 0,
        Player,
        Enemy,
    }
    public int test = 1;
    private int _iScoreTotal;
    //private int _iEnemyLevelUpState;
    //private int _iClearScore;
    private int _iEnemyBreakCount;
    private bool _sendClaerScoreFlg;
    private bool _nextTextMovingFlg;
    private Vector3 _playerFirstPosition = new Vector3(0, 0, -10);
    private Vector3 _playerNowPosition;
    private Vector3 _nextTextMovePos = new Vector3(0, -900, 0);
    private PlayerCtrl _playerObject;
    private BulletSelector.BulletTypeKind _playerSettingTypeBullet;


    public Vector3 GetPlayerNowPosition()
    {
        return _playerNowPosition;
    }
    /*public void ChangeRankUpBoss(int stage)
    {
        //_enemyMangeObject.ChangeEnenmyLevel();
        _enemyMangeObject.enemyRankUp += 1;
        if (stage < 4)
        {
            _nextTextMovingFlg = false;
        }
        _iEnemyBreakCount = 0;
#if UNITY_IPHONE || UNITY_ANDROID
        _playerBulletInterval -= 10;
#endif
        if (stage == 3)
        {
            _playerSettingTypeBullet = BulletSelector.BulletTypeKind.Spread;
        }
    }*/
    private void ChangeSetRankUpState()
    {
        _enemyMangeObject.enemyRankUp += 1;
        _nextTextMovingFlg = false;
        _iEnemyBreakCount = 0;
#if UNITY_IPHONE || UNITY_ANDROID
        _playerBulletInterval -= 10;
#endif
    }
    public void ChangeRankUp(int rank, int stage)
    {
        _iEnemyBreakCount += 1;
        if (rank == 7)
        {
            if (_iEnemyBreakCount >= 1)//ボスは1体だけなので
            {
                ChangeSetRankUpState();
            }
        }
        else
        {
            if (_iEnemyBreakCount >= rank * stage * 3)
            {
                ChangeSetRankUpState();
            }
        }
        if (stage == 3)
        {
            _playerSettingTypeBullet = BulletSelector.BulletTypeKind.Spread;
        }

    }
    public void AddScoreScore(int point, bool flg)
    {
        if (_sendClaerScoreFlg == true)
        {
            _iScoreTotal += point;
            _ScoreText.text = "Score:" + _iScoreTotal.ToString();
            if (flg == true)
            {
                //_ScoreText.text = "Score:" + _iClearScore.ToString();
                _FailureText.enabled = true;
                _FailureText.text = "Clear";
                //_socketMangeObject.SendScoreByREST(_iScoreTotal);
                _sendClaerScoreFlg = false;
                _nextTextMovingFlg = true;
                _nextTextMovePos.y = -900;
                //_playerBulletInterval = 9999999;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //初期化
        //プレイヤーのオブジェクトの設置
        _playerObject = Instantiate(_playerPrefabObject, _playerFirstPosition, Quaternion.identity);
        //プレイヤーの弾設定 
        //_playerObject.thisSettingTypeBullet = BulletSelector.BulletTypeKind.Spread;
        _playerSettingTypeBullet = BulletSelector.BulletTypeKind.Normal;//Spread;//
        //enemyManagerの初期化
        _enemyMangeObject.Init();
        //敵弾が参照する自機位置情報
        _playerNowPosition = _playerFirstPosition;
        //スコア
        _iScoreTotal = 0;
        //終了フラグ
        _FailureText.enabled = false;
        //達成スコア
        //_iClearScore = 93000;
        //敵ランクの更新用
        //_iEnemyLevelUpState = 20;
        _sendClaerScoreFlg = true;
        _nextTextMovingFlg = true;
        _iEnemyBreakCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //更新
        if (_playerObject)
        {
#if UNITY_IPHONE || UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        _playerInputStartPosition = Input.GetTouch(0).position;
                        break;

                    case TouchPhase.Moved://0.5f
                        _playerInputMoveNormalized = (Input.GetTouch(0).position - _playerInputStartPosition).normalized;
                        if (_playerNowPosition.x + _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.x >= _mapLeft &&
                            _playerNowPosition.x + _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.x <= _mapRight)
                        {
                            _playerNowPosition.x += _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.x;
                            _playerObject.MovePositionSide(_playerSpeed * Time.deltaTime * _playerInputMoveNormalized.x);
                        }
                        if (_playerNowPosition.z + _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.y >= _mapUnder &&
                            _playerNowPosition.z + _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.y <= _mapTop)
                        {
                            _playerNowPosition.z += _playerSpeed * Time.deltaTime * _playerInputMoveNormalized.y;
                            _playerObject.MovePositionVertical(_playerSpeed * Time.deltaTime * _playerInputMoveNormalized.y);
                        }
                        break;
                }
            }
            _playerBulletCount += 80 * Time.deltaTime;
            if (_playerBulletCount >= _playerBulletInterval)
            {
                _playerObject.MakeBullet(MachineTypeKind.Player, _playerObject.pBulletProgressPos, _playerSettingTypeBullet);
                _playerBulletCount = 0;
            }
#else
            //キー入力
            if (Input.GetKey(KeyCode.A))
            {
                if (_playerNowPosition.x + _fLeftMove * Time.deltaTime >= _mapLeft)
                {
                    _playerObject.MovePositionSide(_fLeftMove * Time.deltaTime);
                    _playerNowPosition.x += _fLeftMove * Time.deltaTime;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (_playerNowPosition.x + _fRightMove * Time.deltaTime <= _mapRight)                    
                {
                    _playerObject.MovePositionSide(_fRightMove * Time.deltaTime);
                    _playerNowPosition.x += _fRightMove * Time.deltaTime;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (_playerNowPosition.z + _fUpMove * Time.deltaTime <= _mapTop)
                {
                    _playerObject.MovePositionVertical(_fUpMove * Time.deltaTime);
                    _playerNowPosition.z += _fUpMove * Time.deltaTime;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (_playerNowPosition.z + _fDownMove * Time.deltaTime >= _mapUnder)
                {
                    _playerObject.MovePositionVertical(_fDownMove * Time.deltaTime);
                    _playerNowPosition.z += _fDownMove * Time.deltaTime;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerObject.MakeBullet(MachineTypeKind.Player, _playerObject.pBulletProgressPos, _playerSettingTypeBullet);
            }
            //_playerBulletCount += 80 * Time.deltaTime;
            //if (_playerBulletCount >= _playerBulletInterval)
            //{
            //    _playerObject.MakeBullet(MachineTypeKind.Player, _playerObject.pBulletProgressPos, _playerSettingTypeBullet);
            //    _playerBulletCount = 0;
            //}

#endif
        }
        else
        {
            if (_sendClaerScoreFlg == true)
            {
                _FailureText.enabled = true;
                //_socketMangeObject.SendScoreByREST(_iScoreTotal);
                _sendClaerScoreFlg = false;
            }

        }
        _enemyMangeObject.ChildUpdate(_playerNowPosition);
        if (_nextTextMovingFlg == false)
        {
            if (_enemyMangeObject.enemyRankUp == 6)
            {
                _NextText.text = "Danger";
            }
            else
            {
                _NextText.text = "GoNext";
            }
            if (_NextText.transform.localPosition.y >= 900)
            {
                _nextTextMovingFlg = true;
                _nextTextMovePos.y = -900;
            }
            _nextTextMovePos.y += 700 * Time.deltaTime;
            _NextText.transform.localPosition = _nextTextMovePos;
        }
    }
}
