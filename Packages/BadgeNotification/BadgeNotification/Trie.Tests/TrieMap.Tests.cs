using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Voidex.Trie;

public class TrieMapTests
{
    class ValueClass
    {
        public int Data { get; set; }
    }

    #region Tests

    //test update trie map
    [Test]
    public void UpdateTrieMap01()
    {
        var trie = BuildSampleTrie();
        trie.Add("key1|key12|key123", 1234);
        Assert.AreEqual(1234, trie.ValueBy("key1|key12|key123"));
        trie.Update("key1|key12|key123", 10);
        Assert.AreEqual(10, trie.ValueBy("key1|key12|key123"));
    }

    [Test]
    public void UpdateTrieMap02()
    {
        var trie = BuildSampleTrie();
        //set all value to 0
        var keys = trie.Keys();
        var enumerable = keys as string[] ?? keys.ToArray();
        foreach (var key in enumerable)
        {
            Debug.Log(key);
            trie.Update(key, 0);
        }

        foreach (var key in enumerable)
        {
            Assert.AreEqual(0, trie.ValueBy(key));
        }
    }

    [Test]
    public void ValueBy01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(3, trie.ValueBy("key1|key12|key123"));
        Assert.AreEqual(2, trie.ValueBy("key1|key12"));
        Assert.AreEqual(1, trie.ValueBy("key1"));
    }

    [Test]
    public void ValueBy02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(3, trieO.ValueBy("mod1|mod12|mod123").Data);
        Assert.AreEqual(2, trieO.ValueBy("mod1|mod11").Data);
        Assert.AreEqual(null, trieO.ValueBy("mod"));
    }

    [Test]
    public void Values01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(8, trie.Values().Count());
        var values = new List<int> {1, 2, 3, 4, 5, 6, 7, 8};

        Assert.IsTrue(values.OrderBy(x => x).SequenceEqual(trie.Values().OrderBy(x => x)));
    }

    [Test]
    public void Values02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(13, trieO.Values().Count());
        var values = new HashSet<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};

        Assert.IsTrue
        (
            values.OrderBy(x => x)
                .SequenceEqual(trieO.Values().Select(x => x.Data).OrderBy(x => x))
        );
    }

    [Test]
    public void ValuesBy01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(3, trie.ValuesBy("key1|key12").Count());
        Assert.AreEqual(4, trie.ValuesBy("key1").Count());
    }

    [Test]
    public void ValuesBy02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(1, trieO.ValuesBy("mod1|mod12|mod123").Count());
        Assert.AreEqual(3, trieO.ValuesBy("mod1").Count());
        Assert.AreEqual(3, trieO.ValuesBy("mod4").Count());
        Assert.AreEqual(4, trieO.ValuesBy("mod3|mod31").Count());
    }

    [Test]
    public void Keys01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(8, trie.Keys().Count());
        var keys = new HashSet<string>
        {
            "key1",
            "key1|key12",
            "key1|key12|key123",
            "key1|key12|key123|key1234",
            "key2",
            "key2|key21",
            "key2|key21|key22",
            "key2|key21|key22|key23"
        };
        foreach (var key in trie.Keys())
        {
            Debug.Log(key);
        }

        foreach (var key in trie.Keys())
        {
            Assert.IsTrue(keys.Contains(key));
        }
    }

    [Test]
    public void Keys02()
    {
        var trie0 = BuildSampleTrieO();
        Assert.AreEqual(13, trie0.Keys().Count());
        var keys = new HashSet<string>
        {
            "mod1",
            "mod1|mod11",
            "mod1|mod12|mod123",
            "mod2|mod21|mod211|mod2111",
            "mod2|mod21|mod211|mod2112",
            "mod3|mod31|mod311",
            "mod3|mod31|mod312",
            "mod3|mod31|mod313",
            "mod3|mod31|mod314",
            "mod4|mod41",
            "mod4|mod42",
            "mod4|mod43",
            "mod5"
        };
        foreach (var key in trie0.Keys())
        {
            Debug.Log(key);
        }

        foreach (var key in trie0.Keys())
        {
            Assert.IsTrue(keys.Contains(key));
        }
    }

    [Test]
    public void KeysBy01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(3, trie.KeysBy("key1|key12").Count());
        Assert.AreEqual(4, trie.KeysBy("key2").Count());
        Assert.AreEqual(4, trie.KeysBy("key1").Count());
    }

    [Test]
    public void KeysBy02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(2, trieO.KeysBy("mod2|mod21|mod211").Count());
        Assert.AreEqual(4, trieO.KeysBy("mod3").Count());
        Assert.AreEqual(3, trieO.KeysBy("mod4").Count());
        Assert.AreEqual(3, trieO.KeysBy("mod1").Count());
    }

    [Test]
    public void KeyValuePairs01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(8, trie.KeyValuePairs().Count());
        var keys = new HashSet<string>
        {
            "key1",
            "key1|key12",
            "key1|key12|key123",
            "key1|key12|key123|key1234",
            "key2",
            "key2|key21",
            "key2|key21|key22",
            "key2|key21|key22|key23"
        };
        var pairs = trie.KeyValuePairs();
        var keyValuePairs = pairs as KeyValuePair<string, int>[] ?? pairs.ToArray();
        for (int i = 0; i < keyValuePairs.Count(); i++)
        {
            var pair = keyValuePairs.ElementAt(i);
            Assert.IsTrue(keys.Contains(pair.Key) && pair.Value == i + 1);
        }
    }

    [Test]
    public void KeyValuePairs02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(13, trieO.KeyValuePairs().Count());
        var keys = new HashSet<string>
        {
            "mod1",
            "mod1|mod11",
            "mod1|mod12|mod123",
            "mod2|mod21|mod211|mod2111",
            "mod2|mod21|mod211|mod2112",
            "mod3|mod31|mod311",
            "mod3|mod31|mod312",
            "mod3|mod31|mod313",
            "mod3|mod31|mod314",
            "mod4|mod41",
            "mod4|mod42",
            "mod4|mod43",
            "mod5"
        };
        var pairs = trieO.KeyValuePairs();
        var keyValuePairs = pairs as KeyValuePair<string, ValueClass>[] ?? pairs.ToArray();
        for (int i = 0; i < keyValuePairs.Count(); i++)
        {
            var pair = keyValuePairs.ElementAt(i);
            Assert.IsTrue(keys.Contains(pair.Key) && pair.Value.Data == i + 1);
        }
    }

    [Test]
    public void KeyValuePairsBy01()
    {
        var trie = BuildSampleTrie();
        Assert.AreEqual(4, trie.KeyValuePairsBy("key1").Count());
        var keys = new HashSet<string>
        {
            "key1",
            "key1|key12",
            "key1|key12|key123",
            "key1|key12|key123|key1234"
        };
        var pairs = trie.KeyValuePairsBy("key1");
        var keyValuePairs = pairs as KeyValuePair<string, int>[] ?? pairs.ToArray();
        for (int i = 0; i < keyValuePairs.Length; i++)
        {
            Debug.Log(keyValuePairs[i].Key);
        }
        
        for (int i = 0; i < keyValuePairs.Length; i++)
        {
            var pair = keyValuePairs[i];
            Assert.IsTrue(keys.Contains(pair.Key) && pair.Value == i + 1);
        }
    }

    [Test]
    public void KeyValuePairsBy02()
    {
        var trieO = BuildSampleTrieO();
        Assert.AreEqual(2, trieO.KeyValuePairsBy("mod2").Count());
        var keys = new HashSet<string>
        {
            "mod2|mod21|mod211|mod2111",
            "mod2|mod21|mod211|mod2112"
        };
        var pairs = trieO.KeyValuePairsBy("mod2").ToArray();
        for (int i = 0; i < pairs.Length; i++)
        {
            Debug.Log(pairs[i].Key + " " + pairs[i].Value.Data);
        }
        
        Assert.IsTrue(keys.Contains(pairs[0].Key) && pairs[0].Value.Data == 4);
        Assert.IsTrue(keys.Contains(pairs[1].Key) && pairs[1].Value.Data == 5);
    }

    [Test]
    public void HasKey01()
    {
        var trie = BuildSampleTrie();
        Assert.IsTrue(trie.HasKey("key1"));
        Assert.IsTrue(trie.HasKey("key1|key12"));
        Assert.IsTrue(trie.HasKey("key1|key12|key123"));
        Assert.IsTrue(trie.HasKey("key1|key12|key123|key1234"));
        Assert.IsTrue(trie.HasKey("key2"));
        Assert.IsTrue(trie.HasKey("key2|key21"));
        Assert.IsTrue(trie.HasKey("key2|key21|key22"));
        Assert.IsFalse(trie.HasKey("mod"));
        Assert.IsFalse(trie.HasKey("key"));
    }

    [Test]
    public void HasKey02()
    {
        var trieO = BuildSampleTrieO();
        Assert.IsTrue(trieO.HasKey("mod1"));
        Assert.IsTrue(trieO.HasKey("mod1|mod11"));
        Assert.IsTrue(trieO.HasKey("mod1|mod12|mod123"));
        Assert.IsTrue(trieO.HasKey("mod2|mod21|mod211|mod2111"));
        Assert.IsTrue(trieO.HasKey("mod2|mod21|mod211|mod2112"));
        Assert.IsTrue(trieO.HasKey("mod3|mod31|mod311"));
        Assert.IsTrue(trieO.HasKey("mod3|mod31|mod312"));
        Assert.IsTrue(trieO.HasKey("mod3|mod31|mod313"));
        Assert.IsTrue(trieO.HasKey("mod3|mod31|mod314"));
        Assert.IsTrue(trieO.HasKey("mod4|mod41"));
        Assert.IsTrue(trieO.HasKey("mod4|mod42"));
        Assert.IsTrue(trieO.HasKey("mod4|mod43"));
        Assert.IsTrue(trieO.HasKey("mod5"));
        Assert.IsFalse(trieO.HasKey("mod"));
        Assert.IsFalse(trieO.HasKey("key"));
    }

    [Test]
    public void HasKeyPrefix01()
    {
        var trie = BuildSampleTrie();
        Assert.IsTrue(trie.HasKeyPrefix("key2"));
        Assert.IsTrue(trie.HasKeyPrefix("key1"));
        Assert.IsFalse(trie.HasKeyPrefix("1"));
        Assert.IsFalse(trie.HasKeyPrefix("key"));
    }

    [Test]
    public void HasKeyPrefix02()
    {
        var trieO = BuildSampleTrieO();
        Assert.IsTrue(trieO.HasKeyPrefix("mod1"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod2"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod3"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod4"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod5"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod1|mod11"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod1|mod12"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod1|mod12|mod123"));
        Assert.IsTrue(trieO.HasKeyPrefix("mod2|mod21"));
        Assert.IsFalse(trieO.HasKeyPrefix("mod3|mod31|mod314|mod3141"));
        Assert.IsFalse(trieO.HasKeyPrefix("mod"));
        Assert.IsFalse(trieO.HasKeyPrefix("1"));
    }

    [Test]
    public void Remove01()
    {
        var trie = BuildSampleTrie();
        trie.Remove("key1|key12");
        Assert.IsFalse(trie.HasKey("key12"));
        Assert.Throws<ArgumentOutOfRangeException>(() => trie.Remove("key"));
        Assert.Throws<ArgumentOutOfRangeException>(() => trie.Remove("a"));
    }

    [Test]
    public void Remove02()
    {
        var trie = BuildSampleTrie();
        var trieO = BuildSampleTrieO();
        trieO.Remove("mod1|mod12|mod123");
        Assert.IsFalse(trieO.HasKey("mod1|mod12|mod123"));
        Assert.Throws<ArgumentOutOfRangeException>(() => trie.Remove("key"));
        Assert.Throws<ArgumentOutOfRangeException>(() => trie.Remove("a"));
    }

    [Test]
    public void RemoveKeyPrefix01()
    {
        var trie = BuildSampleTrie();
        Assert.IsTrue(trie.RemoveKeyPrefix("key1|key12"));
        Assert.IsFalse(trie.HasKey("key1|key12"));
        Assert.IsFalse(trie.RemoveKeyPrefix("x"));
        Assert.IsTrue(trie.RemoveKeyPrefix("key1"));
        Assert.AreEqual(4, trie.Values().Count());
    }

    [Test]
    public void RemoveKeyPrefix02()
    {
        var trie = BuildSampleTrie();
        var trieO = BuildSampleTrieO();
        Assert.IsTrue(trie.RemoveKeyPrefix("key2"));
        Assert.IsFalse(trie.HasKey("key2|key21|key22|key23"));
        Assert.IsFalse(trie.RemoveKeyPrefix("abc"));
        Assert.AreEqual(4, trie.Values().Count());
        Assert.IsTrue(trie.RemoveKeyPrefix("key1"));
        Assert.AreEqual(0, trie.Values().Count());
        //----------------------------------------------------//
        trieO.RemoveKeyPrefix("mod1");
        Assert.IsFalse(trieO.HasKey("mod1|mod11"));
        Assert.IsFalse(trieO.HasKey("mod1|mod12|mod123"));
        Assert.IsFalse(trieO.HasKey("mod1|mod12"));
        Assert.AreEqual(10, trieO.Values().Count());
    }

    [Test]
    public void GetLongestKeyValuePairs01()
    {
        var trie = BuildSampleTrie();
        trie.Add("this is the longest word I want to tell you, hihihi!!!!", 1337);
        var expected = new[] {new KeyValuePair<string, int>("this is the longest word I want to tell you, hihihi!!!!", 1337)};
        var longestKeyValuePairs = trie.GetLongestKeyValuePairs();
        Assert.AreEqual(expected, longestKeyValuePairs);
    }

    [Test]
    public void GetLongestKeyValuePairs02()
    {
        var trie = BuildSampleTrie();
        trie.Add("the longest word1----------------------------------", 1337);
        trie.Add("the longest word2----------------------------------", 1337);
        var expected = new[]
        {
            new KeyValuePair<string, int>("the longest word1----------------------------------", 1337),
            new KeyValuePair<string, int>("the longest word2----------------------------------", 1337)
        };
        var longestKeyValuePairs = trie.GetLongestKeyValuePairs();
        Assert.AreEqual(expected, longestKeyValuePairs);
    }

    [Test]
    public void GetShortestKeyValuePairs01()
    {
        var trie = BuildSampleTrie();
        var expected = new[] {new KeyValuePair<string, int>("key1", 1), new KeyValuePair<string, int>("key2", 5)};
        var shortestKeyValuePairs = trie.GetShortestKeyValuePairs();
        Assert.AreEqual(expected, shortestKeyValuePairs);
    }

    [Test]
    public void GetShortestKeyValuePairs02()
    {
        var trie = BuildSampleTrie();
        trie.Add("keya", 1337);
        var expected = new[] {new KeyValuePair<string, int>("key1", 1), new KeyValuePair<string, int>("key2", 5), new KeyValuePair<string, int>("keya", 1337)};
        var shortestKeyValuePairs = trie.GetShortestKeyValuePairs();
        foreach (var keyValuePair in shortestKeyValuePairs)
        {
            Debug.Log(keyValuePair.Key);
        }

        Assert.AreEqual(expected, shortestKeyValuePairs);
    }

    [Test]
    public void Clear01()
    {
        var trie = BuildSampleTrie();
        trie.Clear();
        Assert.IsFalse(trie.HasKey("key123"));
        Assert.AreEqual(0, trie.Values().Count());
    }

    [Test]
    public void Clear02()
    {
        var trieO = BuildSampleTrieO();
        trieO.Clear();
        Assert.IsFalse(trieO.HasKey("key123"));
        Assert.AreEqual(0, trieO.Values().Count());
    }

    [Test]
    public void GetTrieNode_NotNull01()
    {
        var trie = BuildSampleTrie();
        var key1Node = trie.GetTrieNode("key1");
        Assert.IsNotNull(key1Node);
        Assert.AreEqual("key1", key1Node.Word);
        Assert.AreEqual(1, key1Node.Value);
        Assert.IsTrue(key1Node.HasChild("key12"));

        var key12Node = key1Node.GetChild("key12");
        Assert.AreEqual("key12", key12Node.Word);
        // "key12" does not exists
        Assert.AreEqual(2, key12Node.Value);
        var key123Node = key1Node.GetTrieNode("key12|key123");
        Assert.AreEqual("key123", key123Node.Word);
        Assert.AreEqual(3, key123Node.Value);
    }

    [Test]
    public void GetTrieNode_NotNull02()
    {
        var trieO = BuildSampleTrieO();
        var key1Node = trieO.GetTrieNode("mod1");
        Assert.IsNotNull(key1Node);
        Assert.AreEqual("mod1", key1Node.Word);
        Assert.AreEqual(1, key1Node.Value.Data);
        Assert.IsTrue(key1Node.HasChild("mod11"));

        var mod11Node = key1Node.GetChild("mod11");
        Assert.AreEqual("mod11", mod11Node.Word);
        // "key12" does not exists
        Assert.AreEqual(2, mod11Node.Value.Data);

        var mod2111Node = trieO.GetTrieNode("mod2|mod21|mod211|mod2111");
        Assert.AreEqual("mod2111", mod2111Node.Word);
        Assert.IsNotNull(mod2111Node.Value);
        Assert.AreEqual(4, mod2111Node.Value.Data);
    }

    [Test]
    public void GetTrieNode_Null01()
    {
        var trie = BuildSampleTrie();
        var key1Node = trie.GetTrieNode("key1");
        var key1zNode = key1Node.GetChild("z");
        Assert.IsNull(key1zNode);
    }

    [Test]
    public void GetTrieNode_Null02()
    {
        var trieO = BuildSampleTrieO();
        var key1Node = trieO.GetTrieNode("mod1|mod12");
        Assert.IsFalse(key1Node.HasChild("mod"));
        var key1zNode = key1Node.GetChild("mod1|mod12|mod1234");
        Assert.IsNull(key1zNode);
    }

    [Test]
    public void GetRoot01()
    {
        var trie = BuildSampleTrie();
        var root = trie.GetRootTrieNode();
        Assert.IsNotNull(root);
        Assert.AreEqual(string.Empty, root.Word);
    }

    [Test]
    public void GetRoot02()
    {
        var trieO = BuildSampleTrieO();
        var root = trieO.GetRootTrieNode();
        Assert.IsNotNull(root);
        Assert.AreEqual(string.Empty, root.Word);
    }

    #endregion

    private ITrieMap<int> BuildSampleTrie()
    {
        ITrieMap<int> trie = new TrieMap<int>();

        string[] keys = new string[]
        {
            "key1",
            "key1|key12",
            "key1|key12|key123",
            "key1|key12|key123|key1234",
            "key2",
            "key2|key21",
            "key2|key21|key22",
            "key2|key21|key22|key23"
        };

        for (int i = 0; i < keys.Length; i++)
        {
            trie.Add(keys[i], i + 1);
        }

        return trie;
    }

    private ITrieMap<ValueClass> BuildSampleTrieO()
    {
        ITrieMap<ValueClass> trieO = new TrieMap<ValueClass>();
        string[] items =
        {
            "mod1",
            "mod1|mod11",
            "mod1|mod12|mod123",
            "mod2|mod21|mod211|mod2111",
            "mod2|mod21|mod211|mod2112",
            "mod3|mod31|mod311",
            "mod3|mod31|mod312",
            "mod3|mod31|mod313",
            "mod3|mod31|mod314",
            "mod4|mod41",
            "mod4|mod42",
            "mod4|mod43",
            "mod5"
        };
        for (int i = 0; i < items.Length; i++)
        {
            trieO.Add(items[i], new ValueClass {Data = i + 1});
        }

        return trieO;
    }
}