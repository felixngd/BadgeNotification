using System;
using System.Collections.Specialized;
using System.Linq;
using ObservableCollections;
using UnityEngine;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    /// <summary>
    /// Manage character data and item data
    /// </summary>
    public class CharacterManager : MonoBehaviour
    {
        public CharacterData characterData;
        public ItemData itemData;

        private void Start()
        {
            GlobalData.GameResources.Items.CollectionChanged += ItemsOnCollectionChanged;
            Initialize();
            Debug.Log("CharacterManager initialized");
        }

        private void Initialize()
        {
            //init item inventory
            foreach (var itemDataItem in itemData.items)
            {
                GlobalData.GameResources.Items.Add(itemDataItem);
                var prefix = "Root|Characters";
                var postfix = $"Equip|{itemDataItem.name}";
                GlobalData.BadgeNotification.UpdateBadges(prefix, postfix, +1);
            }
            //init characters
            foreach (var item in characterData.characters)
            {
                var character = new Character();
                character.id = item.id;
                character.level = item.level;
                character.upgradeCost = item.upgradeCost;
                character.SetEmptyItems();
                GlobalData.GameResources.Characters.Add(character);
            }
            //init character badges
            InitCharacterBadges();
        }

        private void InitCharacterBadges()
        {
            foreach (var character in GlobalData.GameResources.Characters)
            {
                
                for (int i = 0; i < character.items.Length; i++)
                {
                    //init upgrade item badge
                    var value2 = GlobalData.GameResources.Coin / (10 * ((int) (SlotType) i + 1));
                    GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{character.id}|UpE|{(SlotType) i}", value2);
                }
                //init upgrade character badge
                var value = GlobalData.GameResources.Exp / 20;
                GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{character.id}|UpC", value);
            }
        }

        private void OnDestroy()
        {
            GlobalData.GameResources.Items.CollectionChanged -= ItemsOnCollectionChanged;
        }

        private void ItemsOnCollectionChanged(in NotifyCollectionChangedEventArgs<Item> e)
        {
            BadgesOnCollectionChanged(e);
        }
        ////init equip item badge
        private void BadgesOnCollectionChanged(in NotifyCollectionChangedEventArgs<Item> e)
        {
            // if action is add, add badge to the item slot that have slot type of the item.
            // example of the path of badge of trie: Root|Characters|1|Equip|Sword
            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // var prefix = "Root|Characters";
                    // var postfix = $"Equip|{e.NewItem.name}";
                    // GlobalData.BadgeNotification.UpdateBadges(prefix, postfix, +1);

                    break;
                case NotifyCollectionChangedAction.Remove:
                    // var prefixRemove = "Root|Characters";
                    // var postfixRemove = $"Equip|{e.OldItem.name}";
                    // GlobalData.BadgeNotification.UpdateBadges(prefixRemove, postfixRemove, -1);
                    break;
            }
        }
    }
}