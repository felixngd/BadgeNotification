using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ItemSlot : MonoBehaviour
    {
        public Item Item { get; private set; }
        public TMPro.TextMeshProUGUI itemText;
        public TMPro.TextMeshProUGUI levelText;
        public TMPro.TextMeshProUGUI upgradeCostText;
        public SlotType slotType;

        public Button upgradeButton;

        private CharacterUI _character;

        private void Start()
        {
            var btn = GetComponent<Button>();
            btn.onClick.AddListener(OnClick);
            upgradeButton.onClick.AddListener(UpgradeItem);

            GlobalData.GameResources.CoinChanged += OnCoinChanged;
            _character = GetComponentInParent<CharacterUI>();

            var value = GlobalData.GameResources.Coin / (10 * ((int) slotType + 1));
            if (upgradeCostText != null)
                upgradeCostText.text = ZString.Format("Upgrade({0})", value);
        }

        private void OnCoinChanged(object sender, UserDataChangedEventArgs e)
        {
            //assume the upgrade cost is the multiple of 10 of the slot type
            var value = GlobalData.GameResources.Coin / (10 * ((int) slotType + 1));
            GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{_character.Character.id}|UpE|{slotType}", value);
        }

        private void OnClick()
        {
            if (Item == null) return;
            if (Item.id == -1 || Item == null)
            {
                EquipItem();
            }
            else
            {
                UnequipItem();
            }
        }

        public void Initialize(Item initItem)
        {
            Item = initItem;
            if (string.IsNullOrEmpty(Item.name) || initItem.id == -1)
            {
                itemText.text = "Empty";
                return;
            }

            itemText.text = Item.name;
            levelText.text = Item.level.ToString();
        }

        public void EquipItem()
        {
            var newItem = GlobalData.GameResources.Items.FirstOrDefault(i => i.slotType == slotType);
            if (newItem == null) return;

            Item = newItem;
            Item.isEquipped = (_character.Character.id, true);
            ItemManager.Instance.RemoveItem(newItem);

            itemText.text = Item.name;
            levelText.text = Item.level.ToString();

            itemText.color = Color.white;
            levelText.color = Color.white;

            GetComponent<Image>().color = new Color32(0, 128, 0, 255);
            
            //refresh the badge
            //current slot badge value is set to 0, other slots' value with same type will be -= 1
            GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{_character.Character.id}|Equip|{slotType}", 0);
            
            var prefixRemove = "Root|Characters";
            var postfixRemove = $"Equip|{slotType}";
            GlobalData.BadgeNotification.UpdateBadges(prefixRemove, postfixRemove, -1);
        }

        public void UnequipItem()
        {
            if (Item == null || Item.id == -1) return;
            Item.isEquipped = (-1, false);
            ItemManager.Instance.AddItem(Item);

            SetEmpty();
            //Recalculate the badge value
            var value = GlobalData.GameResources.CountItems(slotType);
            var postfix = $"Equip|{slotType}";
            //update current slot badge, do not update the slot that is equipped
            //get item slots that is equipped
            var listItems = GlobalData.GameResources.Characters.
                SelectMany(c => c.items).Where(i => !i.isEquipped.Item2).ToList();
            
            GlobalData.BadgeNotification.SetBadgesValue("Root|Characters", value, node =>
            {
                var c1 = node.Value.key.EndsWith(postfix);
                var c2 = listItems.Any(i => node.Value.key.EndsWith(i.badgeKey));
                return c1 && c2;
            });
        }

        public void UpgradeItem()
        {
            var i = GlobalData.GameResources.UpgradeItem(Item.id);
            if (i != null)
            {
                levelText.text = i.level.ToString();
            }
        }

        public void SetEmpty()
        {
            Item = new Item {id = -1};
            itemText.text = "Empty";
            levelText.text = string.Empty;
            GetComponent<Image>().color = Color.white;
            itemText.color = Color.black;
            levelText.color = Color.black;
        }
    }
}