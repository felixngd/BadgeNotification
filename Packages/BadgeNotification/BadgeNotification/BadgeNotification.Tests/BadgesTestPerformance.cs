using System.Collections.Generic;
using NUnit.Framework;
using Unity.PerformanceTesting;
using Voidex.Badge.Runtime;

namespace BadgeNotification.BadgeNotification.Tests
{
    public class BadgesTestPerformance
    {
        [SetUp]
        public void Setup()
        {
            BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
        }
        private void GenerateKeys(string prefix, int maxDepth, int currentDepth, List<string> keys, int numKeys)
        {
            if (currentDepth > maxDepth || keys.Count >= numKeys)
            {
                return;
            }

            for (int i = 1; i <= 3; i++)
            {
                if (keys.Count >= numKeys)
                {
                    break;
                }

                string newKey = $"{prefix}|{i}";
                keys.Add(newKey);
                GenerateKeys(newKey, maxDepth, currentDepth + 1, keys, numKeys);
            }
        }
        [Test, Performance]
        public void TestPerformance_Add_1000()
        {
            var keys = new List<string>();
            GenerateKeys("root", 5, 0, keys, 1000);
            BadgesTest.BadgeNotification badge = new BadgesTest.BadgeNotification(keys);
            
            Measure.Method(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        badge.AddBadge(keys[i], i, NodeType.Multiple);
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC().SetUp(() =>
                {
                    BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
                })
                .Run();
        }
        
        [Test, Performance]
        public void TestPerformance_Update_1000()
        {
            var keys = new List<string>();
            GenerateKeys("root", 5, 0, keys, 1000);
            UnityEngine.Debug.Log(keys.Count);
            BadgesTest.BadgeNotification badge = new BadgesTest.BadgeNotification(keys);
            
            for (int i = 0; i < 1000; i++)
            {
                badge.AddBadge(keys[i], i, NodeType.Multiple);
            }

            badge.SetNodeValue("root", NodeType.Multiple);
            Measure.Method(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        badge.UpdateBadge(keys[i], i);
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC().SetUp(() =>
                {
                    BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
                })
                .Run();
        }

        [Test, Performance]
        public void Test_Performance_Update_Badges()
        {
            var keys = new List<string>();
            GenerateKeys("root", 5, 0, keys, 100);
            UnityEngine.Debug.Log(keys.Count);
            BadgesTest.BadgeNotification badge = new BadgesTest.BadgeNotification(keys);
            
            for (int i = 0; i < 100; i++)
            {
                badge.AddBadge(keys[i], i, NodeType.Multiple);
            }

            badge.SetNodeValue("root", NodeType.Multiple);
            
            Measure.Method(() =>
                {
                    badge.UpdateBadges("root", "1", 3);
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC().SetUp(() =>
                {
                    BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
                })
                .Run();
        }
        
        [Test, Performance]
        public void Test_Performance_Update_Badges_2()
        {
            var keys = new List<string>();
            GenerateKeys("root", 5, 0, keys, 1000);
            UnityEngine.Debug.Log(keys.Count);
            BadgesTest.BadgeNotification badge = new BadgesTest.BadgeNotification(keys);
            
            for (int i = 0; i < 1000; i++)
            {
                badge.AddBadge(keys[i], i, NodeType.Multiple);
            }

            badge.SetNodeValue("root", NodeType.Multiple);
            
            Measure.Method(() =>
                {
                    badge.UpdateBadges("root", 4);
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC().SetUp(() =>
                {
                    BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
                })
                .Run();
        }
        
        [Test, Performance]
        public void Test_Performance_Get_Badge_Count()
        {
            var keys = new List<string>();
            GenerateKeys("root", 5, 0, keys, 1000);
            UnityEngine.Debug.Log(keys.Count);
            BadgesTest.BadgeNotification badge = new BadgesTest.BadgeNotification(keys);
            
            for (int i = 0; i < 1000; i++)
            {
                badge.AddBadge(keys[i], i, NodeType.Multiple);
                UnityEngine.Debug.Log(keys[i]);
            }

            badge.SetNodeValue("root", NodeType.Multiple);
            
            Measure.Method(() =>
                {
                    badge.GetBadgeCount("root");
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC().SetUp(() =>
                {
                    BadgeMessaging<int>.Initialize(new BadgesTest.SampleMessaging());
                })
                .Run();
        }
    }
}