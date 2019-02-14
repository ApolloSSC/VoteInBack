using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace VoteIn.IntegrationTest.Shared
{
    public class IntegrationTestBase : IDisposable
    {
        protected static readonly MediaTypeWithQualityHeaderValue JsonMediaType = new MediaTypeWithQualityHeaderValue("application/json");
        protected TestServer Server { get; }
        protected HttpClient Client { get; }

        protected IntegrationTestBase()
        {
            Server = new TestServer(new WebHostBuilder()
                .ConfigureServices(UpdateHostingEnvironment)
                .UseStartup<UnitTestStartup>());

            Server.BaseAddress = new Uri("https://localhost:9208");

            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }

        private static void UpdateHostingEnvironment(IServiceCollection services)
        {
            var hostEnv = services
                .First(s => s.ServiceType == typeof(IHostingEnvironment))
                .ImplementationInstance as IHostingEnvironment;

            hostEnv.EnvironmentName = EnvironmentName.Development;

            var path = AppContext.BaseDirectory;
            var commonPathIndex = path.IndexOf(@"VoteIn.IntegrationTest", StringComparison.Ordinal);
            path = Path.Combine(path.Substring(0, commonPathIndex), @"VoteIn");

            hostEnv.ContentRootPath = path; // The Path is used to load the AppSettings.json and other configuration files

            //Specify to use te assemblies of the webapp see https://github.com/aspnet/Mvc/issues/5992
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            services.AddSingleton(manager);
        }

        protected async Task<string> SendStringResponseAsync(HttpMethod httpMethod, string url, object body)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Accept.Add(JsonMediaType);

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }

            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsStringAsync();
        }

        protected async Task<T> SendJsonResponseAsync<T>(HttpMethod httpMethod, string url, object body)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Accept.Add(JsonMediaType);

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }

            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task SendNoResponseAsync(HttpMethod httpMethod, string url, object body)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Accept.Add(JsonMediaType);

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body, 
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), Encoding.UTF8, "application/json");
            }

            var result = await Client.SendAsync(request);
            result.EnsureSuccessStatusCode();
        }

        protected async Task SendFailResponseAsync(HttpMethod httpMethod, string url, object body, HttpStatusCode expectedHttpStatus, bool addToken = true)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            request.Headers.Accept.Add(JsonMediaType);

            if (body != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            }

            var result = await Client.SendAsync(request);

            if (result.StatusCode != expectedHttpStatus)
            {
                throw new Exception($"Unexpected status code ({result.StatusCode} while {expectedHttpStatus} was expected.");
            }
        }
    }
}
