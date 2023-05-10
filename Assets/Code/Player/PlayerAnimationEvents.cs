using Plugins.AnimationEventHandling;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    public class PlayerAnimationEvents : AnimationEventsBase
    {
        public UnityEvent OnShoot { get; private set; }
        public UnityEvent OnReload { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            OnShoot = new UnityEvent();
            OnReload = new UnityEvent();
        }

        [AnimationEventHandler("Shoot")]
        public void Shoot()
        {
            OnShoot.Invoke();
        }
        
        [AnimationEventHandler("Reload")]
        public void Reload()
        {
            OnReload.Invoke();
        }
        
    }
}