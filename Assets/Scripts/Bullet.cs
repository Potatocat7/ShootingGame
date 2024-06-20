using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _timeCount = 0;
    private const int TimeLimit = 3000;
    private const float bulletSpeed = 35.0f;
    public GameManger.MachineTypeKind bulletMachineType { get; set; }
    public Vector3 progPos { get; set; }
    protected bool destroyThroughFlg = true;
    protected void MoveActionBullet()
    {
        Vector3 changePos = this.transform.position;

        changePos.x += progPos.x * bulletSpeed * Time.deltaTime;
        changePos.z += progPos.z * bulletSpeed * Time.deltaTime;

        this.transform.position = changePos;
    }
    public virtual void Onhit()
    {
        destroyThroughFlg = false;
    }
    protected virtual void Timeup()
    {
        _timeCount += 1;
        if (_timeCount == TimeLimit)
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