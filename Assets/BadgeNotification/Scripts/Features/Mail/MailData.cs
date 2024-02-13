using System.Collections.Generic;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "MailData", menuName = "Badge Notification Sample/MailData")]
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
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(MailData))]
    public class MailDataEditor : UnityEditor.Editor
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
    #endif
}