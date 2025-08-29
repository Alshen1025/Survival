using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    
    public int Stamina, MaxStamina;

    private void Start()
    {
        Stamina = MaxStamina;
        StartCoroutine(DelayStamina());
    }

    IEnumerator DelayStamina()
    {
        yield return new WaitForSeconds(0.02f);
        SetStamina(0, false);
    }
    public void SetStamina(int value, bool GetText = true)
    {
        Stamina += value;
        if(GetText)
        {
            Color color = value > 0 ? Color.green : Color.red;
            Canvas_Handler.instance.GetText(value.ToString(), color);
        }
        Delegate_Handler.OnStaminaChange(Stamina);
    }
}
