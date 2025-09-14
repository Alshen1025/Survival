using UnityEngine;

public class ManagerBase : MonoBehaviour
{
    public static ManagerBase instance = null;

    private void Awake()
    {
        if(instance == null) instance = this;

        buildingManager = GetComponentInChildren<BuildingManager>();
        gameManager = GetComponentInChildren<GameManager>();
        objectManager = GetComponentInChildren<ObjectManager>();
    }

    [HideInInspector] public BuildingManager buildingManager;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public ObjectManager objectManager;
}
 