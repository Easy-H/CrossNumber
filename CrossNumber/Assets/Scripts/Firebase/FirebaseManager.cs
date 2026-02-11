using EasyH;
using System.Runtime.InteropServices;

static class FirebaseWebGLConnectBridge
{

    [DllImport("__Internal")]
    public static extern void FirebaseConnect(string firebaseConfigValue);
}

public class FirebaseManager : Singleton<FirebaseManager>
{
    private bool _isConnect = false;

    public void SetConfig()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif
        if (_isConnect) return;

        FirebaseWebGLConnectBridge.FirebaseConnect(
            FileManager.Instance.FileConnector.
                Read("FirebaseConfig"));

        _isConnect = true;

    }
}
