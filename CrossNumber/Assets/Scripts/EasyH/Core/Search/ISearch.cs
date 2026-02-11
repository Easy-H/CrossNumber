using System.Collections.Generic;
using System;

namespace EasyH
{
    public interface ISearch<T>
    {
        public ISearchResult<T> Search(
            T startState, int maxConstraint, Func<T, bool> isGoalState,
            Func<T, ICollection<T>> getNeighbour);

    }
}