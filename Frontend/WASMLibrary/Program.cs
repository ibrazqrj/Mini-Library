using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using WASMLibrary;
using WASMLibrary.API;
using WASMLibrary.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    var conf = builder.Configuration;

    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = conf.GetValue<bool>("MudBlazor:Snackbar:PreventDuplicates");
    config.SnackbarConfiguration.NewestOnTop = conf.GetValue<bool>("MudBlazor:Snackbar:NewestOnTop");
    config.SnackbarConfiguration.ShowCloseIcon = conf.GetValue<bool>("MudBlazor:Snackbar:ShowCloseIcon");
    config.SnackbarConfiguration.VisibleStateDuration = conf.GetValue<int>("MudBlazor:Snackbar:VisibleStateDuration");
    config.SnackbarConfiguration.HideTransitionDuration = conf.GetValue<int>("MudBlazor:Snackbar:HideTransitionDuration");
    config.SnackbarConfiguration.ShowTransitionDuration = conf.GetValue<int>("MudBlazor:Snackbar:ShowTransitionDuration");
    config.SnackbarConfiguration.SnackbarVariant = conf.GetValue<Variant>("MudBlazor:Snackbar:SnackbarVariant");
});

builder.Services.AddTransient<ApiAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<ApiClient>()
    .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(builder.Configuration.GetValue<string>("AzureAd:AccessTokenScope"));
    options.ProviderOptions.LoginMode = "popup";
    options.UserOptions.RoleClaim = "roles";
});

builder.Services.AddScoped<ILibraryService, LibraryService>();

await builder.Build().RunAsync();
