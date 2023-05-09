using Code.Player;
using Code.Player.Configs;
using Code.Player.Shooting;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Instalelrs
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private PlayerActionConfig playerActionConfig;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerHpController hpController;
        [SerializeField] private ShootController shootController;
        [SerializeField] private RestartView restartView;
        [SerializeField] private Collector collector;
        
        public override void InstallBindings()
        {
            Container.Bind<Joystick>().FromInstance(joystick).AsSingle().NonLazy();
            Container.Bind<PlayerActionConfig>().FromInstance(playerActionConfig).AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle().NonLazy();
            Container.Bind<PlayerHpController>().FromInstance(hpController).AsSingle().NonLazy();
            Container.Bind<ShootController>().FromInstance(shootController).AsSingle().NonLazy();
            Container.Bind<RestartView>().FromInstance(restartView).AsSingle().NonLazy();
            Container.Bind<Collector>().FromInstance(collector).AsSingle().NonLazy();
        }
    }
}