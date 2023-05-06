using System;

namespace Plugins.AnimationEventHandling
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AnimationEventHandlerAttribute : Attribute
    {
        public string AnimationEventName { get; private set; }

        public AnimationEventHandlerAttribute(string animationEventName)
        {
            AnimationEventName = animationEventName;
        }
    }
}