using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    //called in animator
    public void NormalAttack()
    {
        if (transform.parent.TryGetComponent<Boss>(out Boss boss))
        {
            boss.NormalAttack();
        }
    }
    //called in animator
    public void SlamAttack()
    {
        if (transform.parent.TryGetComponent<Boss>(out Boss boss))
        {
            boss.SlamAttack();
        }
    }
}
