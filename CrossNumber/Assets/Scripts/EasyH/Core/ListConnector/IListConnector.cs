using System.Collections.Generic;

namespace EasyH
{
    public interface IListConnector<T>
    {

        public string GetDefaultPath();
        public string GetExtensionName();
        public IList<T> ReadData(string path);
        
    }
}