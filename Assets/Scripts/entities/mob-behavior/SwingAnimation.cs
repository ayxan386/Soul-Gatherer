using System.Collections;
using UnityEngine;

public class SwingAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;
    [SerializeField] private float cooldownDuration;

    private bool canSwing = true;

    public void Swing()
    {
        if (canSwing)
        {
            canSwing = false;
            animator.SetTrigger(triggerName);
            Invoke("ResetCooldown", cooldownDuration);
        }
    }

    private void ResetCooldown()
    {
        canSwing = true;
    }
}