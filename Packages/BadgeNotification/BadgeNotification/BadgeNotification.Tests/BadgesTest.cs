using NUnit.Framework;
using UnityEditor;
using Voidex.Badge.Runtime;
using Voidex.Badge.Sample;

public class BadgesTest
{
    
    [SetUp]
    public void Setup()
    {
        BadgeMessaging.Initialize(new MessagePipeMessaging());
    } 
    
    [TearDown]
    public void ClearUp()
    {
        BadgeMessaging.Initialize(new MessagePipeMessaging());
    }
    
    [Test]
    public void Test_Add_Badge_01()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        if (graph.Length == 0)
        {
            Assert.Fail("No BadgeGraph found");
        }
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        badgeNotification.AddBadge("Root|Mails|Rewarded", 1);
        var badge = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.NotNull(badge);
        Assert.AreEqual(1, badge.value);
    }
    
    [Test]
    public void Test_Add_Badge_02()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        badgeNotification.AddBadge("Root|Characters|Upgrade", 5);
        var badge = badgeNotification.GetBadge("Root|Characters|Upgrade");
        Assert.NotNull(badge);
        Assert.AreEqual(5, badge.value);
    }
    
    [Test]
    public void Test_Update_Badge_01()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        badgeNotification.AddBadge("Root|Mails|Rewarded", 1);
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", -1);
        var badge = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge.value);
    }
    
    [Test]
    public void Test_Update_Badge_02()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        badgeNotification.AddBadge("Root|Mails|Rewarded", 1);
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", 1);
        var badge = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.AreEqual(2, badge.value);
    }
    
    [Test]
    public void Test_Update_Badge_03()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        
        badgeNotification.AddBadge("Root|Quests|Daily|0", 1);
        badgeNotification.UpdateBadge("Root|Quests|Daily|0", -1);
        var badge = badgeNotification.GetBadge("Root|Quests|Daily|0");
        Assert.AreEqual(0, badge.value);
    }

    [Test]
    public void Test_Get_Badge_Value_01()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        
        badgeNotification.AddBadge("Root|Mails|Rewarded", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeValue("Root|Mails|Rewarded"));
        
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", -11);
        var badge = badgeNotification.GetBadgeValue("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge);
        
        badgeNotification.UpdateBadge("Root|Mails|System", 4);
        badge = badgeNotification.GetBadgeValue("Root|Mails|System");
        Assert.AreEqual(4, badge);
        var badge2 = badgeNotification.GetBadge("Root|Mails|Rewarded");
        Assert.AreEqual(0, badge2.value);
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", 1);
        
        badge = badgeNotification.GetBadgeValue("Root|Mails");
        Assert.AreEqual(5, badge);
    }
    
    [Test]
    public void Test_Get_Badge_Value_02()
    {
        BadgeNotification badgeNotification = new BadgeNotification();
        //find graph using AssetDatabase
        var graph = AssetDatabase.FindAssets("t:BadgeGraph");
        //load graph using AssetDatabase
        var badgeGraph = AssetDatabase.LoadAssetAtPath<BadgeGraph>(AssetDatabase.GUIDToAssetPath(graph[0]));
        badgeNotification.Initialize(badgeGraph);
        
        badgeNotification.UpdateBadge("Root|Mails|System", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeValue("Root|Mails|System"));
        badgeNotification.UpdateBadge("Root|Mails|Rewarded", 4);
        Assert.AreEqual(4, badgeNotification.GetBadgeValue("Root|Mails|Rewarded"));
        
        var badge = badgeNotification.GetBadgeValue("Root|Mails");
        Assert.AreEqual(5, badge);
    }
}
