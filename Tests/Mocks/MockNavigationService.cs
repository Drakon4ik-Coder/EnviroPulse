using SET09102_2024_5.Interfaces;
using SET09102_2024_5.Services;

namespace SET09102_2024_5.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public List<string> RouteHistory { get; } = new();
        public List<Type> ViewHistory { get; } = new();
        public List<object?> NavigationParameters { get; } = new();
        public int BackNavigationCount { get; private set; }
        public bool IsFlyoutEnabled { get; private set; }

        public Task<bool> InitializeAsync() => Task.FromResult(true);

        public Task<bool> IsReadyAsync() => Task.FromResult(true);

        public string GetServiceStatus() => "Ready";

        public string GetServiceName() => "Mock Navigation Service";

        public Task NavigateToAsync(string route)
        {
            RouteHistory.Add(route);
            return Task.CompletedTask;
        }

        public Task GoBackAsync()
        {
            BackNavigationCount++;
            return Task.CompletedTask;
        }

        public Task NavigateToLoginAsync() => NavigateToAsync(RouteConstants.LoginPage);

        public Task NavigateToRegisterAsync() => NavigateToAsync(RouteConstants.RegisterPage);

        public Task NavigateToMainPageAsync() => NavigateToAsync(RouteConstants.MainPage);

        public Task NavigateToAdminDashboardAsync() => NavigateToAsync(RouteConstants.AdminDashboardPage);

        public Task NavigateToRoleManagementAsync() => NavigateToAsync(RouteConstants.RoleManagementPage);

        public Task NavigateToUserRoleManagementAsync() => NavigateToAsync(RouteConstants.UserRoleManagementPage);

        public Task NavigateToViewAsync<TView>() where TView : class
        {
            ViewHistory.Add(typeof(TView));
            RouteHistory.Add(typeof(TView).Name);
            return Task.CompletedTask;
        }

        public Task EnableFlyoutAsync()
        {
            IsFlyoutEnabled = true;
            return Task.CompletedTask;
        }

        public Task DisableFlyoutAsync()
        {
            IsFlyoutEnabled = false;
            return Task.CompletedTask;
        }

        public Task<bool> CanNavigateToRouteAsync(string route) => Task.FromResult(true);

        // Legacy helpers retained for older tests that use page types or parameters directly.
        public Task NavigateToAsync<T>(object? parameter = null) where T : class
        {
            ViewHistory.Add(typeof(T));
            RouteHistory.Add(typeof(T).Name);
            NavigationParameters.Add(parameter);
            return Task.CompletedTask;
        }

        public Task NavigateBackAsync() => GoBackAsync();
    }
}
