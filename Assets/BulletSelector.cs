using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelector : MonoBehaviour
{
    [SerializeField]
    private Bullet _normalBulletObject;
    [SerializeField]
    private Bullet _spreadBulletObject;
    [SerializeField]
    private Bullet _powerBulletObject;
    private Bullet _ansTypeBulletObject;

    public enum BulletTypeKind
    {
        None = 0,
        Normal,
        Spread,
        Power
    }
    public Bullet SelectTypeBullet(BulletTypeKind ID)
    {
        switch (ID)
        {
            case BulletTypeKind.Normal:
                _ansTypeBulletObject = _normalBulletObject;
                break;
            case BulletTypeKind.Spread:
                _ansTypeBulletObject = _spreadBulletObject;
                break;
            case BulletTypeKind.Power:
                _ansTypeBulletObject = _powerBulletObject;
                break;
                //case BulletTypeKind.None:
                //    return erroer;

        }
        return _ansTypeBulletObject;
    }

}
