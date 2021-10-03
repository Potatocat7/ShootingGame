using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : Bullet
{
    [SerializeField]
    private Bullet _childBulletObject;
    private Bullet _bulletCloneObject;
    private Vector3[] _childFirstPosition = new Vector3[]
    {
        new Vector3(0, 0, 1),
        //new Vector3(1, 0, 1),
        //new Vector3(-1, 0, 1),
        new Vector3(1, 0, 2),
        new Vector3(-1, 0, 2),
        //new Vector3(2, 0, 1),
        //new Vector3(-2, 0, 1)
    };
    int _itimeCont = 0;
    void MakeBullet(Bullet bullet, GameManger.MachineTypeKind machineType, Vector3 progPos)
    {
        _bulletCloneObject = Instantiate(bullet, this.transform.position, Quaternion.identity);
        _bulletCloneObject.bulletMachineType = machineType;
        _bulletCloneObject.progPos = progPos;
    }
    void MaikeChild()
    {
        for (int i = 0; i < 3; i++)
        {
            MakeBullet(_childBulletObject, this.bulletMachineType,(Quaternion.LookRotation(_childFirstPosition[i]) * base.progPos).normalized);
        }
    }
    public override void Onhit()
    {
        MaikeChild();
        base.destroyThroughFlg = false;
    }
    protected override void Timeup()
    {
        _itimeCont += 1;
        if (_itimeCont == 100)
        {
            MaikeChild();
            base.destroyThroughFlg = false;
        }
    }
    void Update()
    {
        this.MoveActionBullet();
        this.Timeup();
        if (base.destroyThroughFlg == false)
        {
            Destroy(this.gameObject);
        }
    }
}
