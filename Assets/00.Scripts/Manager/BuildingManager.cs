using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] private float rayDistance = 100.0f;
    [SerializeField] private LayerMask layer;
    [HideInInspector]public BuildingObject BuildingObject;

    [SerializeField] private float rotationSpeed = 500.0f;


    float ignoreTime = 0.5f;
    float timer;

    public void SetBuild(Building_Scriptable data)
    {
        BuildingObject = Instantiate(data.Object);
        BuildingObject.Data = data;
        BuildingObject.SetMaterial(MaterialType.Transparent);
        BuildingObject.SetTrigger(true);
        BuildingObject.CanBuild = true;
        timer = Time.time + ignoreTime;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if(BuildingObject == null) return;
   

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, layer)) ;
        {
            BuildingObject.transform.position = hitInfo.point;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0)
        {
            Debug.Log("Rotate!");
            BuildingObject.transform.Rotate(0f, scroll * rotationSpeed * Time.deltaTime, 0f);
        }

        if (Time.time < timer) return;

        if(Input.GetMouseButtonDown(0))
        {
            if (BuildingObject.CanBuild == false) return;
            ConfirmPlacement();
        }
    }

    private void ConfirmPlacement()
    {
        BuildingObject.SetTrigger(false);
        BuildingObject.Comfirm();
        BuildingObject = null;
        
    }
}
