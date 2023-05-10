using Code.Pooling;
using UnityEngine;
using Zenject;

namespace Code.Player
{
    public class BulletsPool: ObjectPool<Bullet>
    {
        public BulletsPool(Bullet prefab, int initialSize,DiContainer container, Transform holder) : base(prefab, initialSize, container, holder)
        {
        }
    }
}