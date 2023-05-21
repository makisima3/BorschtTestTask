using Code.UI.Shop;
using UnityEngine;
using Zenject;

namespace Code.Player.World
{
    public class ShopWorld : MonoBehaviour
    {
        [Inject] private WeaponShopView shopView;

        public void Show()
        {
            shopView.Show();
        }
    }
}