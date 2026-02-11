using System.Collections.Generic;
using System;

namespace EasyH {

    public class AStarSearch<T> : ISearch<T> where T : struct
    {

        private readonly struct AStarNode
        {
            public T State { get; }
            public int GScore { get; }
            public int FScore { get; }

            public AStarNode(T state, int gScore, int fScore)
            {
                State = state;
                GScore = gScore;
                FScore = fScore;
            }
        }

        private Func<T, T, int> _getCost = (from, to) => 1;
        private Func<T, int> _getHeuristic = (state) => 0;

        private IDictionary<T, int> _hScoreCache
            = new Dictionary<T, int>();

        public void SetEdgeCostCalcer(Func<T, T, int> getCost)
        {
            _getCost = getCost ?? ((from, to) => 1);
        }

        public void SetHeuristicCalcer(Func<T, int> getHeuristic)
        {
            _getHeuristic = getHeuristic ?? ((state) => 0);
        }

        private int CalcEdgeCost(T from, T to)
        {
            return _getCost(from, to);
        }

        private int CalcHeuristic(T state)
        {
            if (_hScoreCache.ContainsKey(state))
            {
                return _hScoreCache[state];
            }

            int h = _getHeuristic(state);
            _hScoreCache[state] = h;
            return h;
        }

        public ISearchResult<T> Search(
            T start, int point, Func<T, bool> isGoal,
            Func<T, ICollection<T>> getNeighbor)
        {

            _hScoreCache.Clear();

            IDictionary<T, T?> visitedNodes = new Dictionary<T, T?>();
            IDictionary<T, int> minGScore = new Dictionary<T, int>();

            IQueue<AStarNode> nodesToVisitQueue =
                new HeapQueue<AStarNode>(
                    (a, b) => a.FScore.CompareTo(b.FScore));

            nodesToVisitQueue.Enqueue(
                new AStarNode(start, 0, CalcHeuristic(start)));
            minGScore.Add(start, 0);

            visitedNodes.Add(start, null);

            while (nodesToVisitQueue.Count > 0)
            {
                AStarNode currentNode = nodesToVisitQueue.Dequeue();

                T currentState = currentNode.State;

                if (currentNode.GScore > minGScore[currentState])
                {
                    continue;
                }

                foreach (T pos in getNeighbor(currentState))
                {
                    if (isGoal(pos))
                    {
                        visitedNodes[pos] = currentNode.State;
                        return new SearchResult<T>(visitedNodes);
                    }

                    int newGScore = currentNode.GScore
                        + CalcEdgeCost(currentState, pos);

                    if (newGScore > point)
                        continue;

                    if (minGScore.ContainsKey(pos) &&
                        minGScore[pos] <= newGScore)
                    {
                        continue;
                    }

                    minGScore[pos] = newGScore;
                    visitedNodes[pos] = currentState;

                    nodesToVisitQueue.Enqueue(
                        new AStarNode(pos, newGScore,
                            newGScore + CalcHeuristic(pos)));
                }
            }

            return new SearchResult<T>(visitedNodes);
        }
    }
}