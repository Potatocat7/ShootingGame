using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _timeCount = 0;
    public GameManger.MachineTypeKind bulletMachineType { get; set; }
    public Vector3 progPos { get; set; }
    protected bool destroyThroughFlg = true;
    protected void MoveActionBullet()
    {
        Vector3 changePos = this.transform.position;

        changePos.x += progPos.x * 30.0f * Time.deltaTime;
        changePos.z += progPos.z * 30.0f * Time.deltaTime;

        this.transform.position = changePos;
    }
    public virtual void Onhit()
    {
        destroyThroughFlg = false;
    }
    protected virtual void Timeup()
    {
        _timeCount += 1;
        if (_timeCount == 500)
        {
            destroyThroughFlg = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Onhit();
        }
    }

}