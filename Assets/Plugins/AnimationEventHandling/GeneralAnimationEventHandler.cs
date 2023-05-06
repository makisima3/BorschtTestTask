using UnityEngine;
using UnityEngine.Events;

namespace Plugins.AnimationEventHandling
{
    public class GeneralAnimationEventHandler: MonoBehaviour
    {
        [SerializeField] private UnityEvent<string> onGeneralHandlerInvoked;

        public UnityEvent<string> OnGeneralHandlerInvoked => onGeneralHandlerInvoked;

        internal void Init()
        {
            onGeneralHandlerInvoked = new UnityEvent<string>();
        } 

        /// <summary>
        /// AnimationEventHandling Handler
        /// </summary>
        /// <param name="eventName"></param>
        private void AEHHandler(string eventName)
        {
            onGeneralHandlerInvoked.Invoke(eventName);
        }
    }
}