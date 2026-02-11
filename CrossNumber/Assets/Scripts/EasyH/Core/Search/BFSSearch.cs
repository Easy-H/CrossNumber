using System.Collections.Generic;
using System;

namespace EasyH
{

    public class BFSSearch<T> : ISearch<T> where T : struct
    {
        class SearchInfor
        {
            internal T value;
            internal int depth;

            internal SearchInfor(T v, int d)
            {
                value = v;
                depth = d;
            }
        }

        public ISearchResult<T> Search(
            T start, int maxConstraint, Func<T, bool> isGoal,
            Func<T, ICollection<T>> getNeighbor)
        {
            IDictionary<T, T?> visitedNodes = new Dictionary<T, T?>();

            Queue<SearchInfor> nodesToVisitQueue
                = new Queue<SearchInfor>();

            nodesToVisitQueue.Enqueue(new SearchInfor(start, 0));
            visitedNodes.Add(start, null);

            while (nodesToVisitQueue.Count > 0)
            {
                SearchInfor currentNode = nodesToVisitQueue.Dequeue();

                foreach (T pos in getNeighbor(currentNode.value))
                {
                    if (isGoal(pos))
                    {
                        visitedNodes[pos] = currentNode.value;
                        return new SearchResult<T>(visitedNodes);
                    }

                    if (currentNode.depth + 1 > maxConstraint)
                        continue;

                    if (visitedNodes.ContainsKey(pos))
                        continue;

                    visitedNodes[pos] = currentNode.value;
                    nodesToVisitQueue.Enqueue(
                        new SearchInfor(pos, currentNode.depth + 1));

                }
            }

            return new SearchResult<T>(visitedNodes);
        }
    }
}