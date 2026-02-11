using System;
using System.Collections.Generic;

namespace EasyH.Gaming.TurnBased
{
    public class TurnSystem : ITurnSystem
    {
        private Func<bool> _condition;
        
        public void SetGameProceedCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        private int _turn;
        private IList<Team> _teams;
        private ISet<IObserver<int>> _observers;

        public int TurnSpend => _turn / _teams.Count;
        public int ActiveTeamIdx => _turn % _teams.Count;

        public TurnSystem()
        {
            _teams = new List<Team>();
            _observers = new HashSet<IObserver<int>>();
        }

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Unsubscriber<int>(_observers, observer);
        }

        private void TeamRetire(int retireTeamIdx)
        {
            foreach (IObserver<int> target in _observers) {
                target.OnNext(retireTeamIdx);
            } 
        }

        public void StartGame()
        {
            _teams[0].StartTurn();
        }
        
        public void AddTeamMember(IMemberState m)
        {
            while (m.TeamIdx >= _teams.Count)
            {
                _teams.Add(new Team());
            }
            _teams[m.TeamIdx].AddMember(m);

        }
        
        public void RemoveTeamMember(IMemberState m)
        {
            _teams[m.TeamIdx].RemoveMember(m);

            if (_teams[m.TeamIdx].GetLeftMemberCount() > 0) return;

            TeamRetire(m.TeamIdx);
            TurnEnd();
        }
        
        public void TurnEnd()
        {
            if (_condition != null && !_condition()) return;

            _turn++;
            
            while (_teams[ActiveTeamIdx].GetLeftMemberCount() < 1)
            {
                _turn++;
            }
            
            _teams[ActiveTeamIdx].StartTurn();

        }
    }

}