using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class QuestView : MonoBehaviour
    {
        private Quest _quest;
        public TMPro.TextMeshProUGUI questText;
        public Button button;
        public static readonly Color32[] colors = new Color32[]
        {
            new Color32(191, 226, 255, 255), // Soft Blue: #BFE2FF
            new Color32(224, 224, 224, 255), // Light Gray: #E0E0E0
            new Color32(200, 230, 201, 255)  // Pale Green: #C8E6C9
        };
        
        public void Initialize(Quest q)
        {
            this._quest = q;
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ClaimReward);
            GetComponent<UnityEngine.UI.Image>().color = colors[Random.Range(0, colors.Length)];
            
            button.onClick.AddListener(CompleteQuest);
            if (_quest.isCompleted)
            {
                button.gameObject.SetActive(false);
                questText.text = $"Quest {_quest.id} is completed, click to claim the reward";
            }else
            {
                button.gameObject.SetActive(true);
                questText.text = $"Quest {_quest.id} is not completed, click the Button to complete the quest";
            }
        }

        private void CompleteQuest()
        {
            _quest.isCompleted = true;
            var key = GetBadgeKeyByQuest(_quest);
            GlobalData.BadgeNotification.UpdateBadge(key, _quest.isCompleted ? 1 : 0);
            questText.text = $"Quest {_quest.id} is completed, click to claim the reward";
            button.gameObject.SetActive(false);
        }

        public void ClaimReward()
        {
            //claimed the reward
            if (_quest.isCompleted)
            {
                var key = GetBadgeKeyByQuest(_quest);
                GlobalData.BadgeNotification.UpdateBadge(key, -1);
                questText.text = $"Quest {_quest.id} is completed and claimed the reward";
                
                Debug.Log("key: " + key);
            }
            
            button.gameObject.SetActive(!_quest.isCompleted);
        }

        private string GetBadgeKeyByQuest(Quest quest)
        {
            var key = string.Empty;
            if (quest.questType == Quest.QuestType.Daily)
            {
                key = $"{BadgeConstant.QuestsDaily}|{quest.id}";
            }
            else if (quest.questType == Quest.QuestType.Season)
            {
                key = $"{BadgeConstant.QuestsSeason}|{quest.id}";
            }
            
            return key;
        }
    }
}
