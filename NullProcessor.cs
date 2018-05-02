using System.Threading.Tasks;

public class NullProcessor : IProcessor
{
    public Task DoSomeProcessingStuff(int id)
    {
        return Task.FromResult(0);
    }
}