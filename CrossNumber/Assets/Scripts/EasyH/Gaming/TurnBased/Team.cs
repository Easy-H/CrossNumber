using System.Collections.Generic;

namespace EasyH.Gaming.TurnBased
{
    public class Team
    {

        private IList<IMemberState> _members;

        public Team()
        {
            _members = new List<IMemberState>();
        }

        public int GetLeftMemberCount() => _members.Count;

        public void AddMember(IMemberState c)
        {
            _members.Add(c);
        }

        public void RemoveMember(IMemberState c)
        {
            _members.Remove(c);
        }

        public void StartTurn()
        {
            foreach (IMemberState m in _members)
            {
                m.StartTurn();
            }

        }

        public bool CanNextTurn()
        {
            foreach (IMemberState m in _members)
            {
                if (!m.TurnEnd) return false;
            }
            return true;
        }

    }
}