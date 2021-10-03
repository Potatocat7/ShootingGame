using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField]
    private BulletSelector _bulletSelect;

    private Bullet _bulletCloneObject;
    protected bool onlyOnceHitFlg = false;
    public void MakeBullet(GameManger.MachineTypeKind machineType, Vector3 progPos, BulletSelector.BulletTypeKind thisSettingTypeBullet)
    {
        _bulletCloneObject = Instantiate(_bulletSelect.SelectTypeBullet(thisSettingTypeBullet), this.transform.position, Quaternion.identity);
        _bulletCloneObject.bulletMachineType = machineType;
        _bulletCloneObject.progPos = progPos;
    }

    protected void OnhitDamge()
    {
        Destroy(this.gameObject);
    }
}