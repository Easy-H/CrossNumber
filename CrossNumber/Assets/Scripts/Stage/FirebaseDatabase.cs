using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;

public static class FirestoreWebGLBridge {
    [DllImport("__Internal")]
    public static extern void UploadMap(string name, string value, string objectName, string callback, string fallback);
    [DllImport("__Internal")]
    public static extern void GetJSON(string objectName, string callback, string fallback);
    [DllImport("__Internal")]
    public static extern void GetLevelData(string path, string objectName, string callback);
    [DllImport("__Internal")]
    public static extern void WebAlert(string value);

    [DllImport("__Internal")]
    public static extern void FirestoreConnect(string v1, string v2);
}