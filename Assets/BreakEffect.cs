using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffect : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            Destroy(this.gameObject);
        }
    }
}
