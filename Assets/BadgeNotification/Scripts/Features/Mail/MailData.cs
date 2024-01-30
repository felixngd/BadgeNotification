using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "MailData", menuName = "BadgeNotification/MailData")]
    public class MailData : ScriptableObject
    {
        public List<Mail> mails;
        
        public void RandomData()
        {
            mails = new List<Mail>();
            for (int i = 0; i < 10; i++)
            {
                mails.Add(new Mail
                {
                    id = i,
                    isRead = Random.Range(0, 2) == 1
                });
            }
        }
    }
    [System.Serializable]
    public class Mail
    {
        public int id;
        public bool isRead;
    }
    
    [CustomEditor(typeof(MailData))]
    public class MailDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Random Data"))
            {
                var mailData = (MailData) target;
                mailData.RandomData();
            }
        }
    }
}