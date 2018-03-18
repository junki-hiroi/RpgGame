using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int Id;
    public string Name;
    public BattleActionLibrary.BattleAction Action;
    public int Count;

    public Item()
    {
    }
    
    public Item(Item item)
    {
        Id = item.Id;
        Name = item.Name;
        Action = item.Action;
        Count = item.Count;
    }
}


public class ItemManager
{
    private static ItemManager _instance = null;

    public static readonly Item Yakusou = new Item
    {
        Id = 0,
        Name = "やくそう",
        Action = BattleActionLibrary.CreateRecoverBattleAction(0.3f),
        Count = 0
    };

    public static readonly Item Etel = new Item
    {
        Id = 1,
        Name = "エーテル",
        Action = BattleActionLibrary.CreateRecoverBattleAction(0.8f),
        Count = 0
    };

    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemManager();
                
                
                Item[] items = new Item[]
                {
                    Yakusou,
                    Etel
                };
                
                foreach (Item item in items)
                {
                    Item copiedItem = item;
                    copiedItem.Count = 5;
                    _instance.AddItem(copiedItem);
                }
            }
            return _instance;
        }
    }
        
    public Dictionary<int, Item> Items = new Dictionary<int, Item>();

    public void AddItem(Item item)
    {
        if (Items.ContainsKey(item.Id))
        {
            Items[item.Id].Count = Items[item.Id].Count + item.Count;
        }
        else
        {
            Items.Add(item.Id, item);
        }
        Debug.Log(item.Name + " を買いました. " + Items[item.Id].Count + " 個になりました.");
    }
    
    public void UseItem(Item item)
    {
        if (Items.ContainsKey(item.Id))
        {
            Items[item.Id].Count = Items[item.Id].Count - 1;
            
            Debug.Log(item.Name + " を使いました. " + ItemManager.Instance.Items[item.Id].Count + " 個になりました.");

            if (Items[item.Id].Count == 0)
            {
                Items.Remove(item.Id);
            }
        }
    }
}
