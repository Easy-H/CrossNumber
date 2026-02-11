using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyH.Gaming.TurnBased
{
    public class MemberState : MonoBehaviour, IMemberState
    {
        public bool TurnEnd { get; private set; } = true;
        public int TeamIdx { get; private set; } = -1;

        public Action<bool> OnTurnEndStateChanged { get; set; }
        public Action OnTeamIdxChanged { get; set; }

        public void SetTeamIdx(int idx)
        {
            if (idx == TeamIdx) return;

            if (TeamIdx > 0)
            {
                TurnManager.Instance.System.RemoveTeamMember(this);
            }

            TeamIdx = idx;

            TurnManager.Instance.System.AddTeamMember(this);
        }

        public void Remove()
        { 
            TurnManager.Instance.System.RemoveTeamMember(this);
            
        }

        public void StartTurn()
        {
            TurnEnd = false;
            OnTurnEndStateChanged?.Invoke(false);
        }
        
        public void EndTurn()
        {
            TurnEnd = true;
            TurnManager.Instance.System.TurnEnd();
        }
    }
}