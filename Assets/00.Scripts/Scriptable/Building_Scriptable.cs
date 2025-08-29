using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building_Scriptable", menuName = "Scriptable Objects/Building_Scriptable")]
public class Building_Scriptable : ScriptableObject
{
    public string Name;
    public float Time;
    public List<ITEM> m_items = new List<ITEM>();
    public BuildingObject Object;
}
