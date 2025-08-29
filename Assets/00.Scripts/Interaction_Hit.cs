using UnityEngine;
using System.Collections;


public class Interaction_Hit : M_Object
{
    float shakeAmount = 5.0f;
    float shakeDuration = 0.5f;

    private Quaternion originalRotation;

    public Item itemPrefab;

    private void Start()
    {
        originalRotation = transform.rotation;
        HP = m_Data.HP;
    }


    public override void Interaction()
    {
        Player_Movement.instance.AnimationChange(m_Data.m_Type.ToString());
        Player_Movement.instance.ChangeEquipment(m_Data.m_Type, true);
        base.Interaction();
    }

    public override void OnHit()
    {
        base.OnHit();
        ShakeTree(transform.position - Player_Movement.instance.transform.position);

        if(HP <= 0)
        {
            var items = ItemDrop_Manager.DROPITEMLIST(m_Data.Drop_Items);
            for (int i = 0; i < items.Count;  i++)
            {
                var go = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                go.Init(items[i]);
            }
        }
    }

    private void ShakeTree(Vector3 attackDirection)
    {
        Vector3 oppositeDirection = attackDirection.normalized;

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles.x + oppositeDirection.z * shakeAmount, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z + oppositeDirection.x * shakeAmount);

        StopAllCoroutines();
        StartCoroutine(ShakeAnimation(targetRotation));
    }

    private IEnumerator ShakeAnimation(Quaternion targetRotation)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < shakeDuration/2)
        {
            transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, elapsedTime/(shakeDuration/2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0.0f;
        while (elapsedTime < shakeDuration / 2)
        {
            transform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / (shakeDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
