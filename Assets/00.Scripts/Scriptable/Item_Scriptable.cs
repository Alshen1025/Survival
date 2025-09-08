using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Scriptable", menuName = "Scriptable Objects/Item_Scriptable")]
public class Item_Scriptable : Scriptable_Base
{
    
    public int ItemID;
    public float Weight;

    public Item_Type Type;
    public Rarity rarity;
}

[System.Serializable]
public class ITEM
{
    public Item_Scriptable Data;
    public int Count;
}


