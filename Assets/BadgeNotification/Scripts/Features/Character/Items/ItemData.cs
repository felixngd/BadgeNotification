using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Badge Notification Sample/ItemData")]
    public class ItemData : ScriptableObject
    {
        public List<Item> items;

        public void RandomData()
        {
            string names = "Sword,Shield,Armor,Horse,Helmet,Accessory";
            items = new List<Item>();
            for (int i = 0; i < 5; i++)
            {
                items.Add(new Item
                {
                    id = i,
                    upgradeCost = 10,
                    name = names.Split(',')[i],
                    slotType = (SlotType)Enum.Parse(typeof(SlotType), names.Split(',')[i])
                });
            }
        }

        public ObservableCollection<Item> GetItems()
        {
            return new ObservableCollection<Item>(items);
        }
    }

    public enum SlotType
    {
        Sword,
        Horse,
        Armor,
        Shield,
        Helmet,
        Accessory
    }

    [System.Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int upgradeCost;
        public bool isEquipped;
        public int level;
        public SlotType slotType;
        
        public override string ToString()
        {
            return $"name: {name} - type: {slotType} - level: {level} - cost{upgradeCost} - id: {id}";
        }
    }

    [UnityEditor.CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Random Data"))
            {
                var itemData = (ItemData) target;
                itemData.RandomData();

                //save
                UnityEditor.EditorUtility.SetDirty(itemData);
            }
        }
    }
}