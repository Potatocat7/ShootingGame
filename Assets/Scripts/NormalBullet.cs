using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
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
