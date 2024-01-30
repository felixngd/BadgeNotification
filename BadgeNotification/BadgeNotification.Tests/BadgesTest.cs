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
        badgeNotification.AddBadge("Root|Mail|SecreteMail", 1);
        var badge = badgeNotification.GetBadge("Root|Mail|SecreteMail");
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
        badgeNotification.AddBadge("Root|Character|StepUp", 5);
        var badge = badgeNotification.GetBadge("Root|Character|StepUp");
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
        badgeNotification.AddBadge("Root|Mail|SecreteMail", 1);
        badgeNotification.UpdateBadge("Root|Mail|SecreteMail", -1);
        var badge = badgeNotification.GetBadge("Root|Mail|SecreteMail");
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
        badgeNotification.AddBadge("Root|Mail|SecreteMail", 1);
        badgeNotification.UpdateBadge("Root|Mail|SecreteMail", 1);
        var badge = badgeNotification.GetBadge("Root|Mail|SecreteMail");
        Assert.AreEqual(2, badge.value);
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
        
        badgeNotification.AddBadge("Root|Mail|SecreteMail", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeValue("Root|Mail|SecreteMail"));
        
        badgeNotification.UpdateBadge("Root|Mail|SecreteMail", -11);
        var badge = badgeNotification.GetBadgeValue("Root|Mail|SecreteMail");
        Assert.AreEqual(0, badge);
        
        badgeNotification.UpdateBadge("Root|Mail|SecreteMail", 4);
        badge = badgeNotification.GetBadgeValue("Root|Mail|SecreteMail");
        Assert.AreEqual(4, badge);
        var badge2 = badgeNotification.GetBadge("Root|Mail|RewardedMail");
        Assert.AreEqual(0, badge2.value);
        badgeNotification.UpdateBadge("Root|Mail|RewardedMail", 1);
        
        badge = badgeNotification.GetBadgeValue("Root|Mail");
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
        
        badgeNotification.UpdateBadge("Root|Mail|SystemMail", 1);
        Assert.AreEqual(1, badgeNotification.GetBadgeValue("Root|Mail|SystemMail"));
        badgeNotification.UpdateBadge("Root|Mail|RewardedMail", 4);
        Assert.AreEqual(4, badgeNotification.GetBadgeValue("Root|Mail|RewardedMail"));
        
        var badge = badgeNotification.GetBadgeValue("Root|Mail");
        Assert.AreEqual(5, badge);
    }
}
