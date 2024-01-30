using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;

namespace Voidex.Badge.Sample
{
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] private BadgeGraph badgeGraph;
        
        public static Runtime.BadgeNotification BadgeNotification;

        private void Awake()
        {
            Initialize();
        }

        void Start()
        {
            BadgeNotification = new BadgeNotification();
            BadgeNotification.Initialize(badgeGraph);
            
            //suppose here is where the condition is met
            //mail received and not read
            // BadgeNotification.UpdateBadge("Root|Mail|SystemMail", 6);
            // BadgeNotification.UpdateBadge("Root|Mail|RewardedMail", 1);
            // //quest completed
            // BadgeNotification.UpdateBadge("Root|Quest|Event", 2);
            // BadgeNotification.UpdateBadge("Root|Quest|Daily", 5);
            // BadgeNotification.UpdateBadge("Root|Quest|Season", 3);
            // //character level up
            // BadgeNotification.UpdateBadge("Root|Character|Equip", 3);
            // //weapon upgrade
            // BadgeNotification.UpdateBadge("Root|Character|Upgrade|Armor", 1);
            // BadgeNotification.UpdateBadge("Root|Character|Upgrade|Weapon", 1);
            // BadgeNotification.UpdateBadge("Root|Character|Upgrade|Weapon|Horse", 1);
            // BadgeNotification.UpdateBadge("Root|Character|Upgrade|Weapon|Relic", 1);
        }

        private static void Initialize()
        {
            IPubSub<BadgeChangedMessage> pubSub = new MessagePipeMessaging();
            BadgeMessaging.Initialize(pubSub);
        }
        
        #if ODIN_INSPECTOR
        [Button]
#endif
        public void GetBadgeValue(string key)
        {
            var exists = BadgeNotification.GetBadge(key);
            if (exists == null)
            {
                Debug.Log($"Badge not found: {key}");
                return;
            }

            var badge = BadgeNotification.GetBadgeValue(key);
            Debug.Log($"Badge value: {badge}");
        }
    }
}

