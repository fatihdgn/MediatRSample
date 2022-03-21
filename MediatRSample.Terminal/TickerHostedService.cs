using MediatR;
using MediatRSample.Terminal.Notifications;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediatRSample.Terminal;

public class TickerHostedService : IHostedService, IDisposable
{
    private readonly IOptions<TickerOptions> _options;
    private readonly ILogger<TickerHostedService> _logger;
    private readonly IMediator _mediator;
    private CancellationTokenSource cancellationTokenSource;
    private Task? runningTask;

    public TickerHostedService(IOptions<TickerOptions> options, ILogger<TickerHostedService> logger, IMediator mediator)
    {
        _options = options;
        _logger = logger;
        _mediator = mediator;
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        cancellationTokenSource.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting sample hosted service");
        _logger.LogInformation("Current ticking interval: {CurrentTickingInterval} ms", _options.Value.FromMilliseconds);
        runningTask = RunAsync(cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var count = 0;
        _logger.LogInformation("Started the run");
        while (true)
        {
            await Task.Delay(_options.Value.FromMilliseconds /*milliseconds*/, cancellationToken);
            count++;
            _logger.LogInformation("Triggering the {Count}. tick", count);
            await _mediator.Publish(new TickNotification(count), cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("{Count}. tick triggered", count);
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("The run was cancelled. Stopping the run.");
                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping sample hosted service");
        cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}
