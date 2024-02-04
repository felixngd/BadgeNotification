using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ObservableCollections;
using UnityEngine;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ItemManager : MonoBehaviour
    {
        public GameObject itemPrefab;
        public Transform itemParent;
        public ItemData itemData;
        private List<ItemView> _itemViews = new List<ItemView>();

        public static ItemManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Initialize();
        }
        private void Initialize()
        {
            //clear the parent
            foreach (Transform child in itemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in itemData.items)
            {
                var itemObject = Instantiate(itemPrefab, itemParent);
                var view = itemObject.GetComponent<ItemView>();
                view.Initialize(item);
                _itemViews.Add(view);
            }
        }

        public void AddItem(Item item)
        {
            GlobalData.GameResources.Items.Add(item);
            var itemObject = Instantiate(itemPrefab, itemParent);
            var view = itemObject.GetComponent<ItemView>();
            view.UpdateView(item);
            _itemViews.Add(view);
        }

        public void RemoveItem(Item item)
        {
            GlobalData.GameResources.Items.Remove(item);
            var itemObject = _itemViews.Find(i => i.Item.id == item.id);
            _itemViews.Remove(itemObject);
            Destroy(itemObject.gameObject);
        }
    }
}