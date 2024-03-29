using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Badge Notification Sample/QuestData")]
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

            //save
            EditorUtility.SetDirty(this);
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
#if UNITY_EDITOR
    [CustomEditor(typeof(QuestData))]
    public class QuestDataEditor : UnityEditor.Editor
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
#endif
}