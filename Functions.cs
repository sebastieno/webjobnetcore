using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

public class Functions
{
    private readonly IProcessor processor;

    public Functions(IProcessor processor)
    {
        this.processor = processor;
    }

    public Task ProcessOnTimer([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        return this.processor.DoSomeProcessingStuff(0);
    }

    public Task ProcessQueueMessage([QueueTrigger("queueName")] int id)
    {
        return this.processor.DoSomeProcessingStuff(id);
    }
}
