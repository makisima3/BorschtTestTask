﻿using Code.Pooling;
using Zenject;

namespace Code.Player
{
    public class BulletsPool: ObjectPool<Bullet>
    {
        public BulletsPool(Bullet prefab, int initialSize,DiContainer container) : base(prefab, initialSize, container)
        {
        }
    }
}