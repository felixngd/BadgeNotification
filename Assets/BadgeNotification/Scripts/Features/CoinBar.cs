using UnityEngine;
using UnityEngine.UI;
using Voidex.Badge.Sample.Features.User;

namespace Voidex.Badge.Sample
{
    public class CoinBar : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI coinText;
        public Button addCoinButton;
        public Button subCoinButton;

        private void Start()
        {
            GlobalData.GameResources.CoinChanged += OnCoinChanged;
            coinText.text = GlobalData.GameResources.Coin.ToString();
            addCoinButton.onClick.AddListener(AddCoin);
            subCoinButton.onClick.AddListener(SubCoin);
        }

        private void OnDestroy()
        {
            GlobalData.GameResources.CoinChanged -= OnCoinChanged;
        }

        private void OnCoinChanged(object sender, UserDataChangedEventArgs e)
        {
            coinText.text = e.NewValue.ToString();
        }

        private void AddCoin()
        {
            GlobalData.GameResources.Coin += 10;
        }

        private void SubCoin()
        {
            GlobalData.GameResources.Coin -= 10;
            if (GlobalData.GameResources.Coin < 0)
            {
                GlobalData.GameResources.Coin = 0;
            }
        }
    }
}