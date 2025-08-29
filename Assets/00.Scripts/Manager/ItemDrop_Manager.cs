using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public delegate void onItemGet();

public class ItemDrop_Manager : MonoBehaviour
{
    public static event onItemGet OnItemGet;
    public static Dictionary<int, ITEM> ItemPairs = new Dictionary<int, ITEM>();
    public static float PlayerWeight = 2500.0f;
    public static List<ITEM> DROPITEMLIST(List<ITEMLIST> ItemList)
    {
        List<ITEM> GetItemList = new List<ITEM>();
        for (int i = 0; i < ItemList.Count; i++)
        {
            float RandomValue = Random.Range(0.0f, 100.0f);
            if(RandomValue <= ItemList[i].Value)
            {
                int value = Random.Range(1, ItemList[i].Maximum);

                ITEM item = new ITEM();
                item.Data = ItemList[i].Item_Data;
                item.Count = value;
                GetItemList.Add(item);
            }
        }
        return GetItemList;
    }


    public static void GetITEM(Item_Scriptable scriptableData, int value)
    {
        ITEM item = new ITEM();
        item.Data = scriptableData;
        item.Count = value;

        int ID = item.Data.ItemID;

        //보유중이면
        if (HaveItem(ID))
        {
            ItemPairs[ID].Count += value;
        }
        //보유중이 아니면
        else
        {
            ItemPairs.Add(ID, item);
        }
        OnItemGet?.Invoke();
    }

    public static bool HaveItem(int value)
    {
        if(ItemPairs.ContainsKey(value))
        {
            return true;
        }
        return false;
    }

    public static int ItemCount(int value)
    {
        if (ItemPairs.ContainsKey(value))
        {
            return ItemPairs[value].Count;
        }
        else return 0;
    }

    public static float WeightItem(int key)
    {
        if (HaveItem(key))
        {
            ITEM item = ItemPairs[key];
            float value = item.Data.Weight * item.Count;
            return value;
        }

        return -1.0f;
    }

    public static float Weight()
    {
        float weight = 0.0f;
        foreach (var item in ItemPairs)
        {
            weight += WeightItem(item.Key);
        }
        return weight;
    }

}
