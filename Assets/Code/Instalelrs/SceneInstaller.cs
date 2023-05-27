using Code.Player;
using Code.Player.Configs;
using Code.Player.Data;
using Code.Player.Joysticks;
using Code.Player.Shooting;
using Code.Player.Shooting.Configs;
using Code.UI;
using Code.UI.CollectablesViews;
using Code.UI.CollectablesViews.Configs;
using Code.UI.Shop;
using Code.UI.Weapons;
using UnityEngine;
using Zenject;

namespace Code.Instalelrs
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private MoveJoystick moveJoystick;
        [SerializeField] private ShootJoystick shootJoystick;
        [SerializeField] private PlayerHpController hpController;
        [SerializeField] private ShootController shootController;
        [SerializeField] private RestartView restartView;
        [SerializeField] private Collector collector;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerDataHolder playerDataHolder;
        [SerializeField] private WeaponViewChanger weaponViewChanger;
        [SerializeField] private WeaponShopView shopView;
        [SerializeField] private WeaponChangerView weaponChangerView;
        
        [SerializeField] private BagView bagView;
        [SerializeField] private StorageView storageView;
        [SerializeField] private LootBagView lootBagView;
        
        //Configs
        [SerializeField] private PlayerActionConfig playerActionConfig;
        [SerializeField] private PlayerDataConfig playerDataConfig;
        [SerializeField] private WeaponsConfig weaponsConfig;
        [SerializeField] private PlayerAnimationsConfig playerAnimationsConfig;
        [SerializeField] private CollectablesUIConfig collectablesUIConfig;
        [SerializeField] private WeaponShopConfig shopConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<MoveJoystick>().FromInstance(moveJoystick).AsSingle().NonLazy();
            Container.Bind<ShootJoystick>().FromInstance(shootJoystick).AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle().NonLazy();
            Container.Bind<PlayerHpController>().FromInstance(hpController).AsSingle().NonLazy();
            Container.Bind<ShootController>().FromInstance(shootController).AsSingle().NonLazy();
            Container.Bind<RestartView>().FromInstance(restartView).AsSingle().NonLazy();
            Container.Bind<Collector>().FromInstance(collector).AsSingle().NonLazy();
            Container.Bind<PlayerDataHolder>().FromInstance(playerDataHolder).AsSingle().NonLazy();
            Container.Bind<WeaponViewChanger>().FromInstance(weaponViewChanger).AsSingle().NonLazy();
            Container.Bind<WeaponShopView>().FromInstance(shopView).AsSingle().NonLazy();
            Container.Bind<WeaponChangerView>().FromInstance(weaponChangerView).AsSingle().NonLazy();
            
            Container.Bind<BagView>().FromInstance(bagView).AsSingle().NonLazy();
            Container.Bind<StorageView>().FromInstance(storageView).AsSingle().NonLazy();
            Container.Bind<LootBagView>().FromInstance(lootBagView).AsSingle().NonLazy();
            
            //Configs
            Container.Bind<PlayerActionConfig>().FromInstance(playerActionConfig).AsSingle().NonLazy();
            Container.Bind<PlayerDataConfig>().FromInstance(playerDataConfig).AsSingle().NonLazy();
            Container.Bind<WeaponsConfig>().FromInstance(weaponsConfig).AsSingle().NonLazy();
            Container.Bind<PlayerAnimationsConfig>().FromInstance(playerAnimationsConfig).AsSingle().NonLazy();
            Container.Bind<CollectablesUIConfig>().FromInstance(collectablesUIConfig).AsSingle().NonLazy();
            Container.Bind<WeaponShopConfig>().FromInstance(shopConfig).AsSingle().NonLazy();
        }
    }
}