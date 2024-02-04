using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField] private int characterId;
        [SerializeField] private ItemSlot[] slotViews;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMPro.TextMeshProUGUI levelText;

        public Character Character { get; private set; }

        private void Start()
        {
            Initialize();
            upgradeButton.onClick.AddListener(UpgradeCharacter);

            GlobalData.GameResources.ExpChanged += OnExpChanged;
        }

        private void UpgradeCharacter()
        {
            if (GlobalData.GameResources.Exp < 20) return;

            Character.UpgradeCharacter();
            GlobalData.GameResources.Exp -= 20;
            levelText.text = Character.level.ToString();
        }

        private void Initialize()
        {
            foreach (var item in GlobalData.GameResources.Characters)
            {
                if (item.id == characterId)
                {
                    Character = item;
                    levelText.text = Character.level.ToString();
                    break;
                }
            }

            for (int i = 0; i < slotViews.Length; i++)
            {
                slotViews[i].Initialize(Character.items[i]);
            }
        }

        private void OnExpChanged(object sender, UserDataChangedEventArgs e)
        {
            //for example: 20 exp is needed to upgrade a level of the hero
            var value = GlobalData.GameResources.Exp / 20;

            GlobalData.BadgeNotification.SetBadgeValue($"Root|Characters|{characterId}|UpC", value);
        }
    }
}