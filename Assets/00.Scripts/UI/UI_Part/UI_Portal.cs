using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Portal : UI_Base
{
    public Unit_Panel[] panels;
    public Portal portal;

    [SerializeField] private Image MainIcon;
    [SerializeField] private TextMeshProUGUI MainSpeech;
    [SerializeField] private TextMeshProUGUI MainTitle;

    //필요 재료 UII
    [SerializeField] private GameObject Panel;
    [SerializeField] private Transform Content;


    List<GameObject> Garvage = new List<GameObject>();
    Unit_Scriptable Data;

    private void Start()
    {
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].Init(this);
        }
    }

    public void Init(Portal m_portal)
    {
        portal = m_portal;
    }

    public void SetBuildingObject()
    {
        //생성 가능 여부 확인
        bool CanBuild = true;
        for (int i = 0; i < Data.itemList.Count; i++)
        {
            ITEM item = Data.itemList[i];
            if (ItemDrop_Manager.ItemCount(item.Data.ItemID) < item.Count)
            {
                CanBuild = false;
                break;
            }
        }
        Portal m_portal = new Portal();
        m_portal = portal;

        if (CanBuild == false) return;
        Close();
        m_portal.GetComponent<BuildingObject>().SetMakeData(Data.Key, Data.timer, 
            ()=> m_portal.SpawnWorker());
    }

    public override void Close()
    {
        Delegate_Handler.OnEndInteraction();
        base.Close();
    }

    //추후 수정
    public void SetData(Unit_Scriptable data)
    {
        Data =data ;
        if (Garvage.Count > 0)
        {
            for(int i = 0;i < Garvage.Count; i++)
            {
                Destroy(Garvage[i]);
            }
            Garvage.Clear();
        }
        MainIcon.gameObject.SetActive(true);
        MainTitle.gameObject.SetActive(true);
        MainSpeech.gameObject.SetActive(true);

        MainIcon.sprite = AssetManager.GetAtlas(data.Key);
        MainTitle.text = data.Key;
        MainSpeech.text = data.Key;

        for(int i = 0;i < data.itemList.Count; i++)
        {
            Item_Scriptable itemData = data.itemList[i].Data;
            var go = Instantiate(Panel, Content);
            go.SetActive(true);

            var goText = Utils.FindBase<TextMeshProUGUI>(go.transform, "Count");

            Utils.FindBase<Image>(go.transform, "Icon").sprite = AssetManager.GetAtlas(itemData.Key);
            Utils.FindBase<TextMeshProUGUI>(go.transform, "Title").text = itemData.Key;
            goText.text = string.Format("({0}/{1})", data.itemList[i].Count, ItemDrop_Manager.ItemCount(itemData.ItemID));
            goText.color = ItemDrop_Manager.ItemCount(itemData.ItemID) >= data.itemList[i].Count ? Color.green : Color.red;  

            Garvage.Add(go);   
        }
    }
}
