using EHTool;

public class GameManager : MonoSingleton<GameManager>
{
    public IAuther Auth { get; private set; }
    public IPlayground Playground { get; set; } = new SolvePlayground();

    protected override void OnCreate()
    {
        base.OnCreate();
#if !UNITY_WEBGL || UNITY_EDITOR
        Auth = new FirebaseAuther();
#else
        Auth = GetComponent<FirebaseAuthWebGL>();
        Auth ??= gameObject.AddComponent<FirebaseAuthWebGL>();
#endif
        Auth.Initialize();
    }
}
