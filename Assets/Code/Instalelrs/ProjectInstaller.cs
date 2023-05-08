using Code.Player;
using UnityEngine;
using Zenject;

namespace Code.Instalelrs
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SoundsManager soundsManager;
        public override void InstallBindings()
        {
            Container.Bind<SoundsManager>().FromInstance(soundsManager).AsSingle().NonLazy();
        }
    }
}