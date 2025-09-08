using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class Navigation_Item : MonoBehaviour
{
    [SerializeField] private Image RarityImage;

    [SerializeField] private Image ItemIconImage;

    [SerializeField] private TextMeshProUGUI ItemNameText;



    public void Init(Item_Scriptable m_Data, int count)
    {
        RarityImage.sprite = AssetManager.GetAtlas(m_Data.rarity.ToString());
        ItemIconImage.sprite = AssetManager.GetAtlas(m_Data.ItemID.ToString());
        ItemNameText.text = m_Data.Key + "x" + count.ToString();
    }
}
