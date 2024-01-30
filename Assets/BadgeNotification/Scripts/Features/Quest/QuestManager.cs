using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidex.Badge.Runtime;

namespace Voidex.Badge.Sample
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestData questData;
        public Transform dailyQuestParent;
        public Transform seasonQuestParent;
        public GameObject questPrefab;
        
        private List<Quest> _dailyQuests;
        private List<Quest> _seasonQuests;
        
        public BadgeNode dailyQuestBadgeNode;
        public BadgeNode seasonQuestBadgeNode;
        
        private void Start()
        {
            Initialize();   
            Debug.Log("QuestManager initialized");
        }

        private void Initialize()
        {
            _dailyQuests = questData.dailyQuests;
            _seasonQuests = questData.seasonQuests;
            
            foreach (var quest in _dailyQuests)
            {
                var questObject = Instantiate(questPrefab, dailyQuestParent);
                questObject.GetComponent<QuestView>().Initialize(quest);
                var badgeItem = questObject.AddComponent<DynamicBadgeItem>();
                var key = $"{dailyQuestBadgeNode.GetValue(null)}_{quest.id}";
                badgeItem.SetIdAndSubscribe(key);
                ApplicationContext.BadgeNotification.AddBadge(key, quest.isCompleted ? 0 : 1);
            }
            
            foreach (var quest in _seasonQuests)
            {
                var questObject = Instantiate(questPrefab, seasonQuestParent);
                questObject.GetComponent<QuestView>().Initialize(quest);
                var badgeItem = questObject.AddComponent<DynamicBadgeItem>();
                var key = $"{seasonQuestBadgeNode.GetValue(null)}_{quest.id}";
                badgeItem.SetIdAndSubscribe(key);
                ApplicationContext.BadgeNotification.AddBadge(key, quest.isCompleted ? 0 : 1);
            }
        }
    }
}
