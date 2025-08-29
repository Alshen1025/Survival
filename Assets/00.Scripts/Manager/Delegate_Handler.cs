using UnityEngine;

public delegate void Interactive();
public delegate void Stamina(int value);


public class Delegate_Handler : MonoBehaviour
{
    public static event Interactive OnInteraction;
    public static event Interactive OutInteraction;
    public static event Stamina OnStamina;
    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnEndInteraction() => OutInteraction?.Invoke();

    public static void OnStaminaChange(int value) => OnStamina?.Invoke(value);
}
