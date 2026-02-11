using UnityEngine;

namespace EasyH
{
    public class UnityFileConnector : IFileConnector
    {
        public string Read(string path)
        {
            TextAsset retval
                = (TextAsset)Resources.Load(path, typeof(TextAsset));

            return retval.text;
        }
    }
}