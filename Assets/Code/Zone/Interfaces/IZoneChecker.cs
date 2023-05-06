using Code.Zone.Enums;

namespace Code.Zone.Interfaces
{
    public interface IZoneChecker
    {
        void OnEnterInZone(ActionZone zone);
        void OnExitInZone(ActionZone zone);

    }
}