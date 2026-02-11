using System.Collections.Generic;

namespace EasyH
{
    public interface ISearchResult<T>
    {

        public IList<T> GetPathToState(T destination);
        public bool ContainsState(T position);

        public IEnumerable<T> GetAllState();

    }
}