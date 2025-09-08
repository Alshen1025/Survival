using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BonFire : M_Object
{
    public override void Interaction(Character character)
    {
        base.Interaction(character);
        character.AnimationChange("Sitting");
        StartCoroutine(BonFireCoroutine());
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        StopAllCoroutines();
    }

    IEnumerator BonFireCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        ManagerBase.instance.gameManager.SetStamina(10);
        StartCoroutine(BonFireCoroutine());
    }


}
