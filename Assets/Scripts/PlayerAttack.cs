using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public void Event()
    {
        transform.parent.GetComponent<PlayerCombat>().PerformSwordAttack();
        transform.parent.GetComponent<PlayerMining>().SwingPickaxe();
    }
}
