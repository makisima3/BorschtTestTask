using Plugins.AnimationEventHandling;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Enemies
{
    public class EnemyAnimationEvents : AnimationEventsBase
    {
        public UnityEvent OnHitEvent { get; private set; }
        public UnityEvent OnHitAnimationOverEvent { get; private set; }
        public UnityEvent OnDeathAnimationOverEvent { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            OnHitEvent = new UnityEvent();
            OnHitAnimationOverEvent = new UnityEvent();
            OnDeathAnimationOverEvent = new UnityEvent();
        }

        [AnimationEventHandler("OnHit")]
        public void OnHit() => OnHitEvent.Invoke();

        [AnimationEventHandler("OnHitAnimationOver")]
        public void OnHitAnimationOver() => OnHitAnimationOverEvent.Invoke();

        [AnimationEventHandler("OnDeathAnimationOver")]
        public void OnDeathAnimationOver() => OnDeathAnimationOverEvent.Invoke();

    }
}