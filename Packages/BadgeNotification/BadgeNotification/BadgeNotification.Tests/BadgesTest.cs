using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using Voidex.Badge.Runtime;
using Voidex.Badge.Runtime.Interfaces;
using Voidex.Badge.Sample;
using Voidex.Trie;

public class BadgesTest
{
    public class SampleMessaging : IPubSub<BadgeChangedMessage<int>>
    {
        public void Publish(BadgeChangedMessage<int> topic)
        {
            
        }

        public IDisposable Subscribe(string key, Action<BadgeChangedMessage<int>> callback)
        {
            return null;
        }
    }
    public class BadgeNotification : BadgeNotificationBase<int>
    {
        public BadgeNotification(BadgeGraph badgeGraph) : base(badgeGraph)
        {
        }
        
        public BadgeNotification(List<string> list) : base()
        {
            _trieMap = new TrieMap<BadgeData<int>>();
            foreach (var key in list)
            {
                var value = new BadgeData<int>
                {
                    key = key, badgeCount = 0,
                    value = default, nodeType = default
                };
                _trieMap.Add(key, value);
                BadgeMessaging<int>.UpdateBadge(value);
            }
        }
    }
    
    [SetUp]
    public void Setup()
    {
        BadgeMessaging<int>.Initialize(new SampleMessaging());
    }

    [Test]
    public void Test_Add_Badge_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        if (graph.Length == 0)
        {
            Assert.Fail("No BadgeGraph found");
        }

        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);
        badgeNotification.AddBadge("Root|Mails|Secrete", 1);
        var badge = badgeNotification.GetBadge("Root|Mails|Secrete");
        Assert.NotNull(badge);
        Assert.AreEqual(1, badge.badgeCount);

        badgeNotification.AddBadge("Root|Mails|System|0", 10);
        badge = badgeNotification.GetBadge("Root|Mails|System|0");
        Assert.NotNull(badge);
        Assert.AreEqual(10, badge.badgeCount);
    }

    [Test]
    public void Test_Add_Badge_02()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);
        badgeNotification.AddBadge("Root|Characters|ValueUp", 5);
        var badge = badgeNotification.GetBadge("Root|Characters|ValueUp");
        Assert.NotNull(badge);
        Assert.AreEqual(5, badge.badgeCount);
    }

    [Test]
    public void Test_Update_Badge_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);

        badgeNotification.AddBadge("Root|Mails|Rewarded", 1);
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", -1);
        var badge = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge.badgeCount);
    }

    [Test]
    public void Test_Update_Badge_02()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);
        badgeNotification.AddBadge("Root|Mails|Secrete", 1);
        badgeNotification.UpdateBadge("Root|Mails|Secrete", 1);
        var badge = badgeNotification.GetBadge("Root|Mails|Secrete");
        Assert.AreEqual(2, badge.badgeCount);
    }

    [Test]
    public void Test_Update_Badge_03()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);

        badgeNotification.AddBadge("Root|Quests|Daily|0", 1);
        badgeNotification.UpdateBadge("Root|Quests|Daily|0", -1);
        var badge = badgeNotification.GetBadge("Root|Quests|Daily|0");
        Assert.AreEqual(0, badge.badgeCount);
    }

    [Test]
    public void Test_Get_Badge_Value_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);

        badgeNotification.AddBadge("Root|Mails|Secrete", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Mails|Secrete"));

        badgeNotification.UpdateBadge("Root|Mails|Rewarded", -11);
        var badge = badgeNotification.GetBadgeCount("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge);

        badgeNotification.UpdateBadge("Root|Mails|System", 4);
        badge = badgeNotification.GetBadgeCount("Root|Mails|System");
        Assert.AreEqual(4, badge);
        var badge2 = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge2.badgeCount);
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", 1);

        badge = badgeNotification.GetBadgeCount("Root|Mails");
        Assert.AreEqual(6, badge);
    }

    [Test]
    public void Test_Get_Badge_Value_02()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotificationBase<int> badgeNotification = new BadgeNotification(badgeGraph);

        badgeNotification.UpdateBadge("Root|Mails|System", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Mails|System"));
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", 4);
        Assert.AreEqual(4, badgeNotification.GetBadgeCount("Root|Mails|Rewarded"));

        var badge = badgeNotification.GetBadgeCount("Root|Mails");
        Assert.AreEqual(5, badge);
    }

    [Test]
    public void Test_Update_Badges_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);

        var prefix = "Root|Characters|1|UpE";
        badgeNotification.UpdateBadges(prefix, 1);
        var badge = badgeNotification.GetBadge(prefix);
        Assert.AreEqual(6, badge.badgeCount);
        var root = badgeNotification.GetBadgeCount("Root");
        Assert.AreEqual(6, root);
        var characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(6, characters);
        var slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Sword");
        Assert.AreEqual(1, slot1);
        var slot2 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Horse");
        Assert.AreEqual(1, slot2);
        var slot3 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Armor");
        Assert.AreEqual(1, slot3);
        var slot4 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Shield");
        Assert.AreEqual(1, slot4);
        var slot5 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Helmet");
        Assert.AreEqual(1, slot5);
        var slot6 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Accessory");
        Assert.AreEqual(1, slot6);

        badgeNotification.UpdateBadges("Root|Characters|1|UpE|Sword", 1);
        slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Sword");
        Assert.AreEqual(2, slot1);
        var upE = badgeNotification.GetBadgeCount("Root|Characters|1|UpE");
        Assert.AreEqual(7, upE);
        badgeNotification.UpdateBadges("Root|Characters|1|UpE|Horse", -1);
        slot2 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Horse");
        Assert.AreEqual(0, slot2);
        upE = badgeNotification.GetBadgeCount("Root|Characters|1|UpE");
        Assert.AreEqual(6, upE);
    }

    [Test]
    public void Test_Update_Badges_02()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);


        var prefix = "Root|Characters";
        badgeNotification.UpdateBadges(prefix, 1);
        var badge = badgeNotification.GetBadgeCount(prefix);
        Assert.AreEqual(26, badge);

        var root = badgeNotification.GetBadgeCount("Root");
        Assert.AreEqual(26, root);

        var slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Sword");
        Assert.AreEqual(1, slot1);

        var upC = badgeNotification.GetBadgeCount("Root|Characters|1|UpC");
        Assert.AreEqual(1, upC);
    }

    [Test]
    public void Test_Update_Badges_With_Postfix_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);

        var postfix = "Sword";
        badgeNotification.UpdateBadges("Root|Characters", postfix, 2);
        var upgradeSwordSlot = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Sword");
        Assert.AreEqual(2, upgradeSwordSlot);
        var characters = badgeNotification.GetBadgeCount("Root|Characters");
        //0|UpE|Sword = 2; 1|UpE|Sword = 2; 0|Equip|Sword = 1; 1|Equip|Sword = 1
        Assert.AreEqual(6, characters);

        var equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(2, equip_Slot1);
        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(6, characters);
        var root = badgeNotification.GetBadgeCount("Root");
        Assert.AreEqual(6, root);
    }

    [Test]
    public void Test_Update_Badges_03()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);

        /*
         * All the paths in the form of "Root|Characters|*|Equip|*" are Single node in the trie,
         * so if the node's value is >= 1, their parent node uses 1 as their value to count.
         */
        string prefix = "Root|Characters";
        string postfix = "Equip|Sword";
        badgeNotification.UpdateBadges(prefix, 1);
        Assert.AreEqual(26, badgeNotification.GetBadgeCount("Root|Characters"));

        var characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(26, characters);

        var upE_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Sword");
        Assert.AreEqual(1, upE_Slot1);
        var equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(1, equip_Slot1);

        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(26, characters);

        badgeNotification.UpdateBadges(prefix, postfix, 4);
        equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(5, equip_Slot1);
        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(26, characters);

        badgeNotification.UpdateBadges(prefix, postfix, -1);
        equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(4, equip_Slot1);
        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(26, characters);

        badgeNotification.UpdateBadges(prefix, postfix, -1);
        equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(3, equip_Slot1);
        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(26, characters);

        badgeNotification.UpdateBadges(prefix, postfix, -3);
        equip_Slot1 = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(0, equip_Slot1);
        characters = badgeNotification.GetBadgeCount("Root|Characters");
        Assert.AreEqual(24, characters);
    }

    [Test]
    public void Test_Set_Badge_Value_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);

        //Root|Characters|1|Equip|Sword is single node in the trie
        badgeNotification.SetBadgeCount("Root|Characters|1|Equip|Sword", 5);
        var badge = badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword");
        Assert.AreEqual(5, badge);
        badgeNotification.UpdateBadges("Root|Characters", "UpC", 1);
        badge = badgeNotification.GetBadgeCount("Root|Characters|1|UpC");
        Assert.AreEqual(1, badge);

        Assert.AreEqual(3, badgeNotification.GetBadgeCount("Root|Characters"));
    }

    [Test]
    public void Test_Use_Case_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);
        
        string prefix = "Root|Characters";
        //All the paths in the form of "Root|Characters|*|Equip|*" are Single node in the trie,
        //Inventory
        badgeNotification.UpdateBadges(prefix, "Equip|Sword", 3);
        Assert.AreEqual(3, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword"));
        badgeNotification.UpdateBadges(prefix, "Equip|Horse", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Horse"));
        badgeNotification.UpdateBadges(prefix, "Equip|Armor", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Armor"));
        badgeNotification.UpdateBadges(prefix, "Equip|Shield", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Shield"));
        badgeNotification.UpdateBadges(prefix, "Equip|Helmet", 1);
        
        
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Helmet"));
        Assert.AreEqual(0, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Accessory"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|1|Equip"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|1"));
        Assert.AreEqual(10, badgeNotification.GetBadgeCount("Root|Characters"));
        Assert.AreEqual(3, badgeNotification.GetBadgeCount("Root|Characters|0|Equip|Sword"));
        
        //Equip sword to character 1
        badgeNotification.UpdateBadge("Root|Characters|0|Equip|Sword", -1);
        Assert.AreEqual(2, badgeNotification.GetBadgeCount("Root|Characters|0|Equip|Sword"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|0|Equip"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|0"));
        
        badgeNotification.SetBadgeCount("Root|Characters|1|Equip|Sword", 0);
        Assert.AreEqual(0, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword"));
        Assert.AreEqual(4, badgeNotification.GetBadgeCount("Root|Characters|1|Equip"));
        Assert.AreEqual(9, badgeNotification.GetBadgeCount("Root|Characters"));
        
        //upgrade character 1
        badgeNotification.UpdateBadge("Root|Characters|1|UpC", 2);
        Assert.AreEqual(2, badgeNotification.GetBadgeCount("Root|Characters|1|UpC"));
        Assert.AreEqual(6, badgeNotification.GetBadgeCount("Root|Characters|1"));
        Assert.AreEqual(11, badgeNotification.GetBadgeCount("Root|Characters"));
    }

    [Test]
    public void Test_Set_Badges_Value_01()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);
        
        //All the paths in the form of "Root|Characters|*|Equip|*" are Single node in the trie,
        string prefix = "Root|Characters";
        string postfix = "Equip|Sword";
        badgeNotification.SetBadgesValue(prefix, postfix, 5, default);
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|0|Equip|Sword"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword"));
        Assert.AreEqual(2, badgeNotification.GetBadgeCount("Root|Characters"));
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|0"));
    }
    
    [Test]
    public void Test_Set_Badges_Value_02()
    {
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        BadgeNotification badgeNotification = new BadgeNotification(badgeGraph);
        
        //All the paths in the form of "Root|Characters|*|Equip|*" are Single node in the trie,
        string prefix = "Root|Characters";
        string postfix = "Equip|Sword";
        badgeNotification.SetBadgesValue(prefix, postfix, 5, default);
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|0|Equip|Sword"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters|1|Equip|Sword"));
        Assert.AreEqual(2, badgeNotification.GetBadgeCount("Root|Characters"));
        Assert.AreEqual(1, badgeNotification.GetBadgeCount("Root|Characters|0"));
        
        badgeNotification.SetBadgeCount("Root|Characters|1|UpE|Armor", 3);
        Assert.AreEqual(3, badgeNotification.GetBadgeCount("Root|Characters|1|UpE|Armor"));
        Assert.AreEqual(4, badgeNotification.GetBadgeCount("Root|Characters|1"));
        Assert.AreEqual(5, badgeNotification.GetBadgeCount("Root|Characters"));
    }
}