using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Building : UI_Base
{
    public BuildingPanel BuildingPanel;
    public Transform Content;

    List<BuildingPanel> building_list = new List<BuildingPanel>();
    List<GameObject> Garvage = new List<GameObject>();


    public GameObject ItemClickTap;
    Animator animator;

    [SerializeField] private GameObject BuildingItem;
    [SerializeField] private Transform ItemContent;
    [SerializeField] private TextMeshProUGUI TimerText;

    private Building_Scriptable BuildingObject;

    public bool GetClick = false;

    private void Awake()
    {
        Init();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GetClick =false;
        SetBuilding();
    }

    public void SetBuildingObject()
    {
        bool CanBuild = true;
        for (int i = 0; i < BuildingObject.m_items.Count; i++)
        {
            ITEM item = BuildingObject.m_items[i];
            if(ItemDrop_Manager.ItemCount(item.Data.ItemID) < item.Count)
            {
                CanBuild = false;
                break;
            }
        }
        Debug.Log(CanBuild);
        if (CanBuild == false) return;
        Close();

        ManagerBase.instance.buildingManager.SetBuild(BuildingObject);
    }
    
    public void GetItemData(Building_Scriptable data)
    {
        BuildingObject = data;
        if (Garvage.Count > 0)
        {
            for (int i = 0; i < Garvage.Count; i++)
            {
                Destroy(Garvage[i]);
            }
            Garvage.Clear();
        }

        for (int i = 0; i < data.m_items.Count; i++)
        {
            Item_Scriptable itemData = data.m_items[i].Data;
            var go = Instantiate(BuildingItem, ItemContent);
            go.transform.GetComponentInChildren<Image>().sprite = AssetManager.GetAtlas(data.m_items[i].Data.ItemID.ToString());

            var goText = go.transform.GetComponentInChildren<TextMeshProUGUI>();
            bool have = ItemDrop_Manager.HaveItem(itemData.ItemID);

            goText.text =
                string.Format("({0}/{1})",
                data.m_items[i].Count,
                ItemDrop_Manager.ItemCount(itemData.ItemID));

            bool moreItem = ItemDrop_Manager.ItemCount(itemData.ItemID) >= data.m_items[i].Count;
            goText.color = moreItem ? Color.green : Color.red;

            go.gameObject.SetActive(true);
            Garvage.Add(go);
        }
        TimerText.text = Utils.Timer(data.Time);


    }

    public void SetItemClickImage(BuildingPanel slot)
    {
        ItemClickTap.gameObject.SetActive(true);
        ItemClickTap.transform.SetParent(slot.transform);
        ItemClickTap.transform.localPosition = Vector2.zero;
    }

    public void AnimationChange(string temp)
    {
        animator.SetTrigger(temp);
    }

    void Init()
    {
        var buildings = AssetManager.Buildings;

        for (int i = 0; i < buildings.Length; i++)
        {
            var go = Instantiate(BuildingPanel, Content);
            go.Init(buildings[i], this);
            building_list.Add(go);
        }
    }

    void SetBuilding()
    {
        StartCoroutine(GetOpenCoroutine());
    }

    public void OnDisable()
    {
        for(int i = 0;i < building_list.Count;i++)
        {
            building_list[i].gameObject.SetActive(false);
        }
    }

    IEnumerator GetOpenCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i<building_list.Count;i++)
        {
            building_list[i].SetData();
            yield return new WaitForSeconds(0.02f);
        }
    }


}
