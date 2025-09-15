using UnityEngine;

public delegate void Interactive();
public delegate void Stamina(int value);
public delegate void HP(int hp);
public delegate void OnRainIntensityChanged(float Intensity);

public class Delegate_Handler : MonoBehaviour
{
    public static event Interactive OnInteraction;
    public static event Interactive OutInteraction;
    public static event Stamina OnStamina;
    public static event HP OnHP;
    public static event OnRainIntensityChanged RainIntensityChanged;
    public static void OnStartInteraction() => OnInteraction?.Invoke();
    public static void OnEndInteraction() => OutInteraction?.Invoke();
    public static void OnStaminaChange(int value) => OnStamina?.Invoke(value);
    public static void OnHPChange(int value) => OnHP?.Invoke(value);
    public static void ChangeRainIntensity(float intensity) => RainIntensityChanged?.Invoke(intensity);
}

