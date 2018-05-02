using System;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;

public class InjectableJobActivator : IJobActivator
{
    private readonly IServiceProvider service;

    public InjectableJobActivator(IServiceProvider service)
    {
        this.service = service;
    }

    public T CreateInstance<T>()
    {
        return this.service.GetService<T>();
    }
}