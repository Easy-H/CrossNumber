using System.Collections.Generic;

namespace EasyH
{
    public struct SearchResult<T> : ISearchResult<T> where T : struct
    {
        private IDictionary<T, T?> _visitedNodesDict;

        public SearchResult(IDictionary<T, T?> visitedNodesDict)
        {
            _visitedNodesDict = visitedNodesDict;
        }

        public IList<T> GetPathToState(T destination)
        {
            if (!_visitedNodesDict.ContainsKey(destination))
                return new List<T>();

            List<T> path = new List<T> { destination };

            while (_visitedNodesDict[destination] != null)
            {
                path.Add(_visitedNodesDict[destination].Value);
                destination = _visitedNodesDict[destination].Value;
            }

            path.Reverse();

            return path;
        }

        public bool ContainsState(T position)
        {
            return _visitedNodesDict.ContainsKey(position);
        }

        public IEnumerable<T> GetAllState()
            => _visitedNodesDict?.Keys;

    }
}