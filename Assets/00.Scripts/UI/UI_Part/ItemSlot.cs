using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ITEM m_item;
    public GameObject m_ItemSlot;
    public Image Rarity;
    public Image ItemIcon;
    public TextMeshProUGUI ItemCountText;
    public TextMeshProUGUI ItemWeightText;
    public UI_Inventory parentInventory;

    public void Init(ITEM item, UI_Inventory inven)
    {
        m_item = item;
        parentInventory = inven;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (parentInventory == null) return;
        parentInventory.SetItemClickImage(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentInventory == null) return;

        if (parentInventory.ItemClickImage.activeSelf == true)
        {
            parentInventory.ItemClickImage.SetActive(false);
        }
    }

    public void SetItem()
    {
        m_ItemSlot.gameObject.SetActive(m_item.Data == null ? false : true);
        if (m_item.Data != null)
        {
            Rarity.sprite = AssetManager.GetAtlas(m_item.Data.rarity.ToString());
            ItemIcon.sprite = AssetManager.GetAtlas(m_item.Data.ItemID.ToString());
            ItemCountText.text = m_item.Count.ToString();
            ItemWeightText.text = string.Format("{0:0.0}",ItemDrop_Manager.WeightItem(m_item.Data.ItemID));
        }
        else
        {
            Rarity.sprite = AssetManager.GetAtlas("DefaultSquare");
        }
    }

}
