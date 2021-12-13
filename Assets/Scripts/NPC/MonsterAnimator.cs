using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    [SerializeField] private Animator characaterAnimator;

    void Start()
    {
        characaterAnimator = GetComponent<Animator>();
    }

    public void CharacterWalk()
    {
        characaterAnimator.SetBool("isWalking", true);
    }

    public void CharacterStopWalk()
    {
        characaterAnimator.SetBool("isWalking", false);
    }

    public void CharacterFoundEnemy()
    {
        characaterAnimator.SetBool("isFoundEnemy", true);
    }
    public void CharacterNotFoundEnemy()
    {
        characaterAnimator.SetBool("isFoundEnemy", false);
    }

    public void CharacterAttackEnemy()
    {
        characaterAnimator.SetTrigger("isAttackEnemy");
    }

}
