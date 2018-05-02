using System.Threading.Tasks;

public interface IProcessor {
    Task DoSomeProcessingStuff(int id);
}