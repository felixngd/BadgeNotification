using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using Voidex.Badge.Runtime;
using Voidex.Trie;

namespace BadgeNotification.Trie.Tests
{
    public class TriePerformanceTests
    {
        [Test, Performance]
        public void TestPerformance_Add_1000()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            Measure.Method(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        trieMap.Add(keys[i], new BadgeData<int>
                        {
                            //key = keys[i],
                            value = i,
                            badgeCount = i,
                        });
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        private void GenerateKeys(string prefix, int maxDepth, int currentDepth, List<string> keys)
        {
            if (currentDepth > maxDepth)
            {
                return;
            }

            for (int i = 1; i <= 3; i++)
            {
                string newKey = $"{prefix}|{i}";
                keys.Add(newKey);
                GenerateKeys(newKey, maxDepth, currentDepth + 1, keys);
            }
        }

        [Test, Performance]
        public void TestPerformance_Remove_1000()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    foreach (var key in trieMap.Keys())
                    {
                        trieMap.Remove(key);
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestPerformance_Get_1000()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    foreach (var key in trieMap.Keys())
                    {
                        var _ = trieMap.GetTrieNode(key);
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestPerformance_Get_Leaf_Nodes()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    _ = trieMap.GetLeafNodes();
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        [Test, Performance]
        public void TestPerformance_Key_Value_Pairs_By_Root()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    foreach (var pair in trieMap.KeyValuePairsBy(Const.ROOT))
                    {
                        _ = pair.Value;
                        _ = pair.Key;
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }
        
        [Test, Performance]
        public void TestPerformance_Update()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    trieMap.Update(keys[0], new BadgeData<int>()
                    {
                        value = 9999, badgeCount = 9, //key = keys[0]
                    });
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }
        
        [Test, Performance]
        public void TestPerformance_Update_1000()
        {
            var trieMap = new TrieMap<BadgeData<int>>();
            var keys = new List<string>();
            GenerateKeys("Root", 10, 0, keys);
            for (int i = 0; i < 1000; i++)
            {
                trieMap.Add(keys[i], new BadgeData<int>
                {
                    //key = keys[i],
                    value = i,
                    badgeCount = i,
                });
            }

            Measure.Method(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        trieMap.Update(keys[i], new BadgeData<int>()
                        {
                            value = 9999, badgeCount = 9, //key = keys[i]
                        });
                    }
                })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }
    }
}