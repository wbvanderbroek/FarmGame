using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private int damage  = 10;
    public void PerformSwordAttack()
    {
        GetComponent<Enemy>().TakeDamage(damage);
    }
}
