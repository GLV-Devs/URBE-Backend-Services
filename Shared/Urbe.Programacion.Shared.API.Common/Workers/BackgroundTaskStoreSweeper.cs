using Microsoft.Extensions.Hosting;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.Shared.API.Common.Workers;

public class BackgroundTaskStoreSweeper : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            await BackgroundTaskStore.Sweep(null, stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
