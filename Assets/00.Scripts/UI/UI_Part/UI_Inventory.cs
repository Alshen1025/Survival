using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UI_Base
{
    public ItemSlot ItemSlot;
    public Transform Content;

    public UnityEngine.UI.Image WeightFill;
    public TextMeshProUGUI WeightText;

    private List<ItemSlot> items = new List<ItemSlot>();

    Dictionary<int, ITEM> inventoryItems = new Dictionary<int, ITEM>();

    private int ItemMaximumValue = 50;

    public GameObject ItemClickImage;

    private void Start()
    {
        Init();
        ItemDrop_Manager.OnItemGet += SetItemList;
        ItemDrop_Manager.OnItemGet += SetInventory;
        
    }

    public void Init()
    {
        if (ItemDrop_Manager.ItemPairs.Count >= ItemMaximumValue)
        {
            ItemMaximumValue = ItemDrop_Manager.ItemPairs.Count;
        }

        for (int i = 0; i < ItemMaximumValue; i++)
        {
            var go = Instantiate(ItemSlot, Content);
            go.gameObject.SetActive(true);
            items.Add(go);
        }

        SetItemList();
        SetInventory();
    }

    public void SetItemList()
    {
        int value = 0;
        foreach (var item in ItemDrop_Manager.ItemPairs)
        {
            if (inventoryItems.ContainsKey(item.Value.Data.ItemID) == false
                && items[value].parentInventory == null)
            {
                items[value].Init(item.Value, this);
                inventoryItems.Add(item.Value.Data.ItemID, item.Value);
            }
            value++;
        }
    }


    public void SetInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetItem();
        }

        WeightFill.fillAmount = ItemDrop_Manager.Weight() / ItemDrop_Manager.PlayerWeight;
        WeightText.text = string.Format("{0:0.0}/{1:0.0}", ItemDrop_Manager.Weight(), ItemDrop_Manager.PlayerWeight);
    }

    public void SetItemClickImage(ItemSlot slot)
    {
        ItemClickImage.gameObject.SetActive(true);
        ItemClickImage.transform.SetParent(slot.transform);
        ItemClickImage.transform.localPosition = Vector2.zero;
    }
}

