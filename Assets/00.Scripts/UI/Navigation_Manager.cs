using System;
using UnityEngine;

public class Navigation_Manager : MonoBehaviour
{
    public static Navigation_Manager Instance = null;

    private void Awake()
    {
        if(Instance == null) Instance =this;
    }

    [SerializeField] private Transform Content;
    private Navigation_Item Item;

    private void Start()
    {
        Item = GetComponentInChildren<Navigation_Item>();
        Item.gameObject.SetActive(false);
    }

    public void CreateItemPanel(Item_Scriptable data, int count)
    {
        var go = Instantiate(Item, Content);
        go.gameObject.SetActive(true);
        go.Init(data, count);
    }
}
