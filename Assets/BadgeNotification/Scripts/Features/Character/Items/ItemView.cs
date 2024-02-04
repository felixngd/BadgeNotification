using UnityEngine;

namespace Voidex.Badge.Sample
{
    public class ItemView : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI itemName;
        public TMPro.TextMeshProUGUI level;
        readonly Color _softGreen = new Color(0.564f, 0.933f, 0.564f, 1f);
        
        public Item Item { get; private set; }
        public void Initialize(Item item)
        { 
            Item = item;
            itemName.text = item.name;
            level.text = item.level.ToString();
        }
        
        public void UpdateView(Item item)
        {
            Item = item;
            itemName.text = item.name;
            level.text = item.level.ToString();
        }
    }
}