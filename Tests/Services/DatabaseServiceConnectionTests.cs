using Microsoft.EntityFrameworkCore;
using Moq;
using SET09102_2024_5.Data;
using SET09102_2024_5.Interfaces;
using SET09102_2024_5.Services;

namespace SET09102_2024_5.Tests
{
    public class DatabaseServiceConnectionTests
    {
        private static DbContextOptions<SensorMonitoringContext> CreateOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<SensorMonitoringContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        [Fact]
        public async Task InitializeDatabaseAsync_WithInMemoryProvider_CreatesDatabase()
        {
            var options = CreateOptions($"DbInit_{Guid.NewGuid():N}");
            var loggingService = new Mock<ILoggingService>();
            var service = new DatabaseInitializationService(options, loggingService.Object);

            await service.InitializeDatabaseAsync();

            await using var context = new SensorMonitoringContext(options);
            Assert.True(await context.Database.CanConnectAsync());
        }

        [Fact]
        public async Task TestConnectionAsync_WithInMemoryProvider_ReturnsFalse()
        {
            var options = CreateOptions($"DbConnection_{Guid.NewGuid():N}");
            var loggingService = new Mock<ILoggingService>();
            var service = new DatabaseInitializationService(options, loggingService.Object);

            var result = await service.TestConnectionAsync();

            Assert.False(result);
        }
    }
}
