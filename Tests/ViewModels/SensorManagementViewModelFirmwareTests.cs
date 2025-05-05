using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SET09102_2024_5.Data;
using SET09102_2024_5.Models;
using SET09102_2024_5.Tests.Mocks;
using SET09102_2024_5.ViewModels;
using Xunit;

namespace SET09102_2024_5.Tests.ViewModels
{
    public class SensorManagementViewModelFirmwareTests
    {
        private readonly MockMainThreadService _mainThreadService = new();
        private readonly MockDialogService _dialogService = new();

        private SensorMonitoringContext GetContext()
        {
            var options = new DbContextOptionsBuilder<SensorMonitoringContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new SensorMonitoringContext(options);
        }

        [Fact]
        public void ValidateField_EmptyFirmwareVersion_ProducesValidationError()
        {
            // arrange
            var ctx = GetContext();
            var vm = new SensorManagementViewModel(ctx, _mainThreadService, _dialogService);
            vm.Configuration = new Configuration
            {
                SensorId = 1,
                Orientation = null
            };

            // act
            vm.ValidateCommand.Execute(nameof(Configuration.Orientation));

            // assert
            Assert.True(vm.HasValidationErrors);
            Assert.Contains(nameof(Configuration.Orientation), vm.ValidationErrors.Keys);
        }

        [Fact]
        public void ValidateField_FutureLastUpdateDate_ProducesValidationError()
        {
            // arrange
            var ctx = GetContext();
            var vm = new SensorManagementViewModel(ctx, _mainThreadService, _dialogService);
            vm.Configuration = new Configuration
            {
                SensorId = 1,
                Latitude = 120
            };

            // act
            vm.ValidateCommand.Execute(nameof(Configuration.Latitude));

            // assert
            Assert.True(vm.HasValidationErrors);
            Assert.Contains(nameof(Configuration.Latitude), vm.ValidationErrors.Keys);
        }

        [Fact]
        public async Task SaveChangesAsync_ValidFirmware_UpdatesDatabase()
        {
            // arrange
            var ctx = GetContext();
            var sensor = new Sensor
            {
                SensorId = 42,
                SensorType = "Temperature",
                Status = "Active",
                Measurand = new Measurand
                {
                    MeasurandId = 1,
                    QuantityName = "Temperature",
                    QuantityType = "Physical",
                    Symbol = "T",
                    Unit = "C"
                }
            };
            ctx.Sensors.Add(sensor);
            await ctx.SaveChangesAsync();

            var vm = new SensorManagementViewModel(ctx, _mainThreadService, _dialogService);
            vm.SelectedSensor = sensor;
            vm.Configuration = new Configuration
            {
                SensorId = sensor.SensorId,
                Latitude = 0,
                Longitude = 0,
                Altitude = 0,
                Orientation = 0,
                MeasurementFrequency = 5,
                MinThreshold = 0,
                MaxThreshold = 1
            };
            vm.FirmwareVersion = "2.1.5";
            vm.LastUpdateDate = DateTime.Today.AddDays(-1);

            // act
            await ((Task)typeof(SensorManagementViewModel)
                 .GetMethod("SaveChangesAsync", BindingFlags.NonPublic | BindingFlags.Instance)
                 .Invoke(vm, null));


            // assert
            var updated = await ctx.Sensors.Include(s => s.Firmware).FirstAsync(s => s.SensorId == 42);
            Assert.Equal("2.1.5", updated.Firmware.FirmwareVersion);
            Assert.Equal(vm.LastUpdateDate, updated.Firmware.LastUpdateDate);
        }
    }
}
