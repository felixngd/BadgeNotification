using System.Linq;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Extender;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ItemSlot : MonoBehaviour
    {
        public Item Item;
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
                upgradeCostText.SetTextFormat("Upgrade({0})", value);
        }

        private void OnCoinChanged(object sender, UserDataChangedEventArgs e)
        {
            SetBadgeEquipmentUpgrade();
        }

        private void SetBadgeEquipmentUpgrade()
        {
            if (Item.id != -1)
            {
                var value = GlobalData.GameResources.Coin / 10;
                upgradeCostText.SetTextFormat("Upgrade({0})", value);
                GlobalData.BadgeNotification.SetBadgeCount($"Root|Characters|{_character.Character.id}|UpE|{slotType}", value);
            }else
            {
                GlobalData.BadgeNotification.SetBadgeCount($"Root|Characters|{_character.Character.id}|UpE|{slotType}", 0);
            }
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

            //equipped slot badge = 0, badge rank = item level
            GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{_character.Character.id}|Equip|{slotType}", 0, new BadgeValue() {rank = Item.level});

            var prefixRemove = "Root|Characters";
            var postfixRemove = $"Equip|{slotType}";
            //badge from other character with same slot type will be subtracted
            GlobalData.BadgeNotification.UpdateBadgesCount(prefixRemove, -1, node =>
            {
                var c1 = node.Path.EndsWith($"{_character.Character.id}|{postfixRemove}");
                var c2 = node.Path.EndsWith(postfixRemove);
                var c3 = node.Value.badgeCount > 0;
                return !c1 && c2 && c3;
            });
            
            SetBadgeEquipmentUpgrade();
        }

        public void UnequipItem()
        {
            if (Item == null || Item.id == -1) return;
            Item.isEquipped = (-1, false);
            var newItem = Item;
            ItemManager.Instance.AddItem(newItem);
            Item = new Item {id = -1};

            SetEmpty();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //recalculate the badges with same slot type
            var prefix = "Root|Characters";
            var postfix = $"Equip|{slotType}";
            var value = GlobalData.GameResources.CountItems(slotType);
            //unequip set rank for itself to 0, badge count = 0
            GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{_character.Character.id}|Equip|{slotType}", value, new BadgeValue() {rank = 0});

            //highest level
            var maxLevel = GlobalData.GameResources.Items.Where(i => i.slotType == slotType).Max(i => i.level);
            //set the badge slot badgeCount = 1 if the item on the slot is less than the max level
            GlobalData.BadgeNotification.SetBadgesValue(prefix, data => { data.badgeCount = 1; }, node =>
            {
                var c1 = node.Path.EndsWith(postfix);
                var c2 = node.Value.value.rank < maxLevel;
                var c3 = Item.isEquipped.equipped;
                return c1 && c2 && c3;
            });

            SetBadgeEquipmentUpgrade();
            stopwatch.Stop();
            
            Debug.Log($"UnequipItem: {stopwatch.ElapsedMilliseconds}");
        }

        public void UpgradeItem()
        {
            if (Item == null || Item.id == -1) return;

            GlobalData.GameResources.Upgrade(Item);
            levelText.SetText(Item.level.ToString());
        }

        public void SetEmpty()
        {
            Item = new Item {id = -1};
            itemText.SetText(slotType);
            levelText.SetText(string.Empty);
            GetComponent<Image>().color = Color.white;
            itemText.color = Color.black;
            levelText.color = Color.black;
        }
    }
}