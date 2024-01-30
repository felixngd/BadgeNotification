using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    public class QuestView : MonoBehaviour
    {
        public Quest quest;
        public TMPro.TextMeshProUGUI questText;
        
        public void Initialize(Quest q)
        {
            this.quest = q;
            questText.text = $"Quest {quest.id} is {(quest.isCompleted ? "completed" : "not completed")}";
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpdateQuest);
            //randomly color for the Image component, make the color good for the eyes
            GetComponent<UnityEngine.UI.Image>().color = Random.ColorHSV();
        }
        
        public void UpdateQuest()
        {
            quest.isCompleted = !quest.isCompleted;
            questText.text = $"Quest {quest.id} is {(quest.isCompleted ? "completed" : "not completed")}";
        }
    }
}
