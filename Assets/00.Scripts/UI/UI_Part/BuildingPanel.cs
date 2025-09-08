using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Building_Scriptable m_Data;

    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Text;

    public UI_Building parentPanel;


    public void Init(Building_Scriptable Data, UI_Building building)
    {
        m_Data = Data;
        parentPanel = building;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(parentPanel.GetClick == false)
        {
            parentPanel.GetClick = true;
            parentPanel.AnimationChange("Click");
        }
        parentPanel.GetItemData(m_Data);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentPanel == null) return;
        parentPanel.SetItemClickImage(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentPanel == null) return;

        if (parentPanel.ItemClickTap.activeSelf == true)
        {
            parentPanel.ItemClickTap.SetActive(false);
        }
    }

    public void SetData()
    {
        gameObject.SetActive(true);
        Icon.sprite = AssetManager.GetAtlas(m_Data.Key);
        Text.text = m_Data.Key;
    }


    
}
