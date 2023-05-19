using System.Collections.Generic;
using Code.Player.Data;

namespace Code.Collectables.Interfaces
{
    public interface IStorage
    {
        void AddCollectable(CollectableData data);
        void AddCollectables(List<CollectableData> data);
        void RemoveCollectable(CollectableData data);
        void RemoveCollectables(List<CollectableData> data);
        void Open();
        
        List<CollectableData> Collectables { get; }
    }
}