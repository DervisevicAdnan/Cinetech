namespace CineTech.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notifikacijaService = scope.ServiceProvider.GetRequiredService<NotifikacijeServis>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    await notifikacijaService.NotifyAsync();
                    await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); // Proverava svakog dana
                }
            }
        }
    }
}
