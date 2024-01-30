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

                var badgeItem = questObject.GetComponentInChildren<DynamicBadgeItem>();
                var key = $"{dailyQuestBadgeNode.GetValue(null)}|{quest.id}";
                badgeItem.Subscribe(key);
                ApplicationContext.BadgeNotification.AddBadge(key, quest.isCompleted ? 1 : 0);
            }
            
            foreach (var quest in _seasonQuests)
            {
                var questObject = Instantiate(questPrefab, seasonQuestParent);
                questObject.GetComponent<QuestView>().Initialize(quest);
                var badgeItem = questObject.GetComponentInChildren<DynamicBadgeItem>();
                var key = $"{seasonQuestBadgeNode.GetValue(null)}|{quest.id}";
                badgeItem.Subscribe(key);
                ApplicationContext.BadgeNotification.AddBadge(key, quest.isCompleted ? 1 : 0);
            }
        }
    }
}
