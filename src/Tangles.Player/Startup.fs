module Tangles.Player.Startup

open System
open System.Net.Http
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Microsoft.Extensions.DependencyInjection

[<EntryPoint>]
let main args =
    let builder = WebAssemblyHostBuilder.CreateDefault args
    builder.RootComponents.Add<Main.App>("#app")
    builder.Services.AddScoped<HttpClient>(fun _ -> new HttpClient(BaseAddress = Uri builder.HostEnvironment.BaseAddress)) |> ignore
    builder.Build().RunAsync() |> ignore
    0
