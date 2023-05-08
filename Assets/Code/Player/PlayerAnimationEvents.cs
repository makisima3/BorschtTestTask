using Plugins.AnimationEventHandling;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Player
{
    public class PlayerAnimationEvents : AnimationEventsBase
    {
        public UnityEvent OnShoot { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            OnShoot = new UnityEvent();
        }

        [AnimationEventHandler("Shoot")]
        public void Shoot()
        {
            OnShoot.Invoke();
        }
        
    }
}