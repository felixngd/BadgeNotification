using System;
using System.Collections.Generic;
using ObservableCollections;

namespace Voidex.Badge.Sample.Features.User
{
    public class GameResources
    {
        private int _exp;
        private int _coin;

        public int Exp
        {
            get => _exp;
            set
            {
                if (_exp != value)
                {
                    var oldValue = _exp;
                    _exp = value;
                    OnExpChanged(new UserDataChangedEventArgs() {OldValue = oldValue, NewValue = _exp});
                }
            }
        }

        public event EventHandler<UserDataChangedEventArgs> ExpChanged;

        protected void OnExpChanged(UserDataChangedEventArgs e)
        {
            ExpChanged?.Invoke(this, e);
        }

        public int Coin
        {
            get => _coin;
            set
            {
                if (_coin != value)
                {
                    var oldValue = _coin;
                    _coin = value;
                    OnCoinChanged(new UserDataChangedEventArgs() {OldValue = oldValue, NewValue = _coin});
                }
            }
        }

        public event EventHandler<UserDataChangedEventArgs> CoinChanged;

        protected void OnCoinChanged(UserDataChangedEventArgs e)
        {
            CoinChanged?.Invoke(this, e);
        }

        public ObservableList<Item> Items = new ObservableList<Item>();
        
        public ObservableList<Character> Characters = new ObservableList<Character>();

        public Item UpgradeItem(int id)
        {
            Item myItem = null;
            foreach (var item in Items)
            {
                if (item.id == id)
                {
                    myItem = item;
                }
            }

            if (myItem == null) return null;
            if (Coin < 10) return null;
            Coin -= 10;
            myItem.level++;
            return myItem;
        }
    }
}