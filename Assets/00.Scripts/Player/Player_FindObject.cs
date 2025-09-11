using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_FindObject : MonoBehaviour
{
    [SerializeField] private float checkRadius = 5.0f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] GameObject uiPrefab;
    

    [SerializeField] private float activationDistance = 3.0f;

    private Dictionary<Transform, GameObject> activeUI = new Dictionary<Transform, GameObject>();
    public bool OnInteraction = false;
    Transform closeObject;



    void Start()
    {
        Delegate_Handler.OnInteraction += OnInteractionVoid;
        Delegate_Handler.OutInteraction += OnEndInteraction;
    }

    void OnInteractionVoid()
    {
        OnInteraction = true;
        transform.LookAt(closeObject.transform.position);
        closeObject = null;
        UIInit();
    }

    public void OnEndInteraction()
    {
        OnInteraction = false;
        Player_Movement.instance.DeactiveEquipment();
        activeUI.Clear();
    }

    void Update()
    {
        if (OnInteraction) return;

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, checkRadius, interactableLayer);

        closeObject = null;
        float closeDistance = Mathf.Infinity;

        foreach (Collider obj in nearbyObjects)
        {
            Transform targetTransform = obj.transform;
            float distance = Vector3.Distance(transform.position, targetTransform.position);

            if (distance <= activationDistance && distance < closeDistance)
            {
                
                closeObject = targetTransform;
                closeDistance = distance;

            }
        }

        bool isMouseOverUI = Canvas_Handler.IsPointerOverUIObject();
        if (isMouseOverUI)
        {
            closeObject = null;
        }

        if (closeObject != null)
        {
            ShowUI(closeObject);
           

            if (Input.GetKeyDown(KeyCode.F))
            {
                M_Object subObject = null;
                if (closeObject.GetComponent<M_Object>() == null)
                {
                    subObject = closeObject.transform.parent.GetComponent<M_Object>();
                }
                else subObject = closeObject.GetComponent<M_Object>();

                subObject.Interaction(GetComponent<Character>());
                Delegate_Handler.OnStartInteraction();
            }
        }

        UIInit();
    }
    private void UIInit()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (var UIEntry in activeUI)
        {
            if (UIEntry.Key != closeObject || closeObject == null)
            {
                UIEntry.Value.GetComponent<UI_AnimationHandler>().AnimationChange("Out");
                toRemove.Add(UIEntry.Key);
            }
        }
        foreach (var transformToRemove in toRemove)
        {
            activeUI.Remove(transformToRemove);
        }
    }


    private void ShowUI(Transform targetTransform)
    {
        if (activeUI.ContainsKey(targetTransform))
        {
            UpdateIconPosition(targetTransform, activeUI[targetTransform]);
            return;
        }
        GameObject UIInstance = Instantiate(uiPrefab, uiCanvas.transform);
        activeUI[targetTransform] = UIInstance;
        UpdateIconPosition(targetTransform, UIInstance);
    }

    //아이콘이 보여지고 있을 때 카메라가 이동하면 그에 따라 UI업데이트
    private void UpdateIconPosition(Transform targetTransform, GameObject Ui)
    {
        //오브젝트의 좌표가 Canvas상 좌표에서 어디쯤에 위치하는지 계산
        //계산한 위치에 UI표시
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(
            new Vector3(targetTransform.position.x, targetTransform.position.y + 1.5f,
            targetTransform.position.z));
        Ui.GetComponent<RectTransform>().position = screenPosition;
    }
}
