using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    public static ManagerBase instance = null;

    private void Awake()
    {
        if(instance == null) instance = this;

        buildingManager = GetComponentInChildren<BuildingManager>();
        gameManager = GetComponentInChildren<GameManager>();
    }

    [HideInInspector] public BuildingManager buildingManager;
    [HideInInspector] public GameManager gameManager;
}
