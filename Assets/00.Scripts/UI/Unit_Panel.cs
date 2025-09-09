using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit_Panel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Unit_Scriptable Data;
    private UI_Portal parentPanel;
    
    public void Init(UI_Portal parent_Data)
    {
        parentPanel = parent_Data;
    }

    public void SetPanel()
    {
        parentPanel.SetData(Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}
