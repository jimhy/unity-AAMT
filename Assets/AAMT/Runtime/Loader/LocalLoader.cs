namespace AAMT
{
    public class LocalLoader : ILoader
    {
        public LoaderHandler Load(string[] path)
        {
            return LoadLocalTask.GetTask(path).Run();
        }
    }
}