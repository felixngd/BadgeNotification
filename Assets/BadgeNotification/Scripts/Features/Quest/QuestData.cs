using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "BadgeNotification/QuestData")]
    public class QuestData : ScriptableObject
    {
        public List<Quest> dailyQuests;
        public List<Quest> seasonQuests;
        
        public void RandomData()
        {
            dailyQuests = new List<Quest>();
            seasonQuests = new List<Quest>();
            for (int i = 0; i < 10; i++)
            {
                dailyQuests.Add(new Quest
                {
                    id = i,
                    isCompleted = Random.Range(0, 2) == 1, questType = Quest.QuestType.Daily
                });
                seasonQuests.Add(new Quest
                {
                    id = i,
                    isCompleted = Random.Range(0, 2) == 1, questType = Quest.QuestType.Season
                });
            }
        }
    }
    [System.Serializable]
    public class Quest
    {
        public int id;
        public bool isCompleted;
        public QuestType questType;
        
        public enum QuestType
        {
            Daily,
            Season
        }
    }
    
    [CustomEditor(typeof(QuestData))]
    public class QuestDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Random Data"))
            {
                var questData = (QuestData) target;
                questData.RandomData();
            }
        }
    }
}
