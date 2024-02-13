using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class ExpBar : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI expText;
        public Button addExpButton;
        public Button subExpButton;
        
        private void Start()
        {
            GlobalData.GameResources.ExpChanged += OnExpChanged;
            expText.text = GlobalData.GameResources.Exp.ToString();
            addExpButton.onClick.AddListener(AddExp);
            subExpButton.onClick.AddListener(SubExp);
        }
        
        private void OnDestroy()
        {
            GlobalData.GameResources.ExpChanged -= OnExpChanged;
        }
        
        private void OnExpChanged(object sender, UserDataChangedEventArgs e)
        {
            expText.text = e.NewValue.ToString();
        }
        
        private void AddExp()
        {
            GlobalData.GameResources.Exp += 10;
        }
        
        private void SubExp()
        {
            GlobalData.GameResources.Exp -= 10;
            if (GlobalData.GameResources.Exp < 0)
            {
                GlobalData.GameResources.Exp = 0;
            }
        }
    }
}
