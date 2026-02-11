using EasyH;

namespace EasyH.Gaming.TurnBased
{
    public class TurnManager : Singleton<TurnManager>
    {
        public ITurnSystem System { get; set; } = new TurnSystem();
    }
}