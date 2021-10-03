using UnityEngine;


public class PlayerCtrl : Machine
{
    public int bulletIDnumPlayer { get; } = 1;
    public Vector3 pBulletProgressPos { get; } = new Vector3(0,0,1);//自弾の進行方向
    public Vector3 GetPlayerObjectPosition()
    {
        return this.transform.position;
    }

    //自機の操縦操作
    public void MovePositionSide(float step)
    {
        Vector3 changePos = this.transform.position;
        changePos.x += step;
        this.transform.position = changePos;
    }
    public void MovePositionVertical(float step)
    {
        Vector3 changePos = this.transform.position;
        changePos.z += step;
        this.transform.position = changePos;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            this.OnhitDamge();
        }
        else if(other.gameObject.tag == "Finish")
        {
        }
        else if (other.gameObject.GetComponent<Bullet>().bulletMachineType == GameManger.MachineTypeKind.Player)
        {
            //自機の弾では反応しない
        }
        else if (other.gameObject.GetComponent<Bullet>().bulletMachineType == GameManger.MachineTypeKind.Enemy)
        {
            other.GetComponent<Bullet>().Onhit();
            this.OnhitDamge();
        }
    }

}
