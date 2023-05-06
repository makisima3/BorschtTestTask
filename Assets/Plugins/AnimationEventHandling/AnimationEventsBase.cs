using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Plugins.AnimationEventHandling
{
    public abstract class AnimationEventsBase : MonoBehaviour
    {
        [SerializeField] private Animator targetAnimator;
        
        private Dictionary<string, MethodInfo> _eventNameToMethodInfo;
        
        protected virtual void Awake()
        {
            ResolveGeneralHandler();
                
            _eventNameToMethodInfo = GetType().GetRuntimeMethods()
                .Select(methodInfo => new { methodInfo, attribute = AnimationEventHandlerFilter(methodInfo) })
                .Where(methodInfo => methodInfo.attribute != null)
                .ToDictionary(
                    methodInfo => methodInfo.attribute.AnimationEventName, 
                    methodInfo => methodInfo.methodInfo
                );
        }

        private void ResolveGeneralHandler()
        {
            var generalHandler = targetAnimator.GetComponent<GeneralAnimationEventHandler>(); 
            
            if(generalHandler == null)
            {
                generalHandler = targetAnimator.gameObject.AddComponent<GeneralAnimationEventHandler>();
                generalHandler.Init();
            }
            
            generalHandler.OnGeneralHandlerInvoked.AddListener(OnGeneralHandlerInvoked);
        }
        
        private void OnGeneralHandlerInvoked(string eventName)
        {
            if (_eventNameToMethodInfo.TryGetValue(eventName, out var methodInfo))
            {
                InvokeMethodInfo(methodInfo);
            }
        }

        private void InvokeMethodInfo(MethodInfo methodInfo)
        {
            methodInfo?.Invoke(this, Array.Empty<object>());
        }

        private AnimationEventHandlerAttribute AnimationEventHandlerFilter(MethodInfo methodInfo)
        {
            var animationEventHandlerAttribute = methodInfo.GetCustomAttribute<AnimationEventHandlerAttribute>();
            return animationEventHandlerAttribute;
        }
    }
}