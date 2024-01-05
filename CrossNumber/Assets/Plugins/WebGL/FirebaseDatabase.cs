using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void UploadMap(string name, string value, string objectName, string callback, string fallback);
        [DllImport("__Internal")]
        public static extern void GetJSON(string objectName, string callback, string fallback);
        [DllImport("__Internal")]
        public static extern void GetLevelData(string path, string objectName, string callback);
        [DllImport("__Internal")]
        public static extern void WebAlert(string value);

    }
}