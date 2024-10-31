using EHTool;

public class GameManager : Singleton<GameManager> {

    public IPlayground Playground { get; set; } = new SolvePlayground();

}
