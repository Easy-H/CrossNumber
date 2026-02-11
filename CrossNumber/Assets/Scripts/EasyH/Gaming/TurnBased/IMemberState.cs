using System;

namespace EasyH.Gaming.TurnBased
{
    public interface IMemberState
    {
        public bool TurnEnd { get; }
        public int TeamIdx { get; }

        public Action<bool> OnTurnEndStateChanged { get; set; }
        public Action OnTeamIdxChanged { get; set; }
    
        public void SetTeamIdx(int idx);
        public void Remove();

        public void StartTurn();
        public void EndTurn();

    }
}