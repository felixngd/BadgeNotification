using System.Collections;
using System.Collections.Generic;
using ObservableCollections;
using UnityEngine;

namespace Voidex.Badge.Sample
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Badge Notification Sample/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public List<Character> characters;

        public void RandomData()
        {
            characters = new List<Character>();
            for (int i = 0; i < 2; i++)
            {
                var c = new Character
                {
                    id = i,
                    upgradeCost = 20
                };
                c.SetEmptyItems();
                characters.Add(c);
            }
        }
    }

    [System.Serializable]
    public class Character
    {
        public int id;
        public int upgradeCost;
        public int level;
        public Item[] items = new Item[6];
        public void SetEmptyItems()
        {
            for (int i = 0; i < 6; i++)
            {
                items[i] = new Item()
                {
                    id = -1
                };
            }
        }

        public void UpgradeCharacter()
        {
            level++;
        }
        
        public void EquipItem(int index, Item item)
        {
            items[index] = item;
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(CharacterData))]
    public class CharacterDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Random Data"))
            {
                var characterData = (CharacterData) target;
                characterData.RandomData();
                //save
                UnityEditor.EditorUtility.SetDirty(characterData);
            }
        }
    }
#endif
}