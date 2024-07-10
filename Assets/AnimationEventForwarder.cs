using UnityEngine;

public class AnimationEventForwarder : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Start()
    {
        // Find the EnemyAI component on the parent object
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    // This function will be called by the animation event
    public void ForwardDealDamage()
    {
        if (enemyAI != null)
        {
            enemyAI.DealDamage();
        }
    }
}