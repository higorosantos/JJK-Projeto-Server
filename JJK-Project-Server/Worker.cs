using JJK_Project_Server.Core.Network;

namespace JJK_Project_Server;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private GameServer _gameServer;

  public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

    //while (!stoppingToken.IsCancellationRequested)
    //{
    //    if (_logger.IsEnabled(LogLevel.Information))
    //    {
    //      this._gameServer.PollEvents();
    //      //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
    //    }
    //    await Task.Delay((1000 / 30), stoppingToken);
    //}

    _gameServer = new GameServer();
    _gameServer.Start(4000);

    while (!stoppingToken.IsCancellationRequested)
    {
      //_gameServer.PrintStatsAtTop();
      _gameServer.PollEvents();
      await Task.Delay(1000 / 30, stoppingToken);
    }
  }
}
