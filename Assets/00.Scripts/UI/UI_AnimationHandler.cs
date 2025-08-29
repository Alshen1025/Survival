using UnityEngine;

public class UI_AnimationHandler : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    public void Destroy_Object() => Destroy(gameObject);

    public void Deactive() => gameObject.SetActive(false);

}
