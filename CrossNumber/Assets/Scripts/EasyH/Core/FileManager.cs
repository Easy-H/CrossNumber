namespace EasyH
{
    public class FileManager : Singleton<FileManager>
    {
        public IFileConnector FileConnector { get; set; }

        protected override void OnCreate()
        {
            FileConnector = new UnityFileConnector();
        }
    }
}