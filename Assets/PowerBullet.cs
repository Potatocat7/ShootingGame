using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBullet : Bullet
{
    public override void Onhit()
    {
        //this.onhit();
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
