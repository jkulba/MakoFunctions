using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mako
{
    public static class Ping
    {
        private static string _invocationId;

        [FunctionName("Ping")]
        [OpenApiOperation(operationId: "Ping", tags: new[] { "Ping" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns JSON response")]
        public static async Task<IActionResult> Run([
            HttpTrigger(AuthorizationLevel.Function, "get", Route = "ping")] HttpRequest req,
            ExecutionContext executionContext, ILogger log)
        {
            _invocationId = executionContext.InvocationId.ToString();

            var config = GetConfig(executionContext);
            string appVersion = config.GetValue<string>("app_version");
            string buildRun = config.GetValue<string>("build_run");
            string buildCommitHash = config.GetValue<string>("build_commit_hash");
            string buildBranch = config.GetValue<string>("build_branch");
            string buildDate = config.GetValue<string>("build_date");
            string builtBy = config.GetValue<string>("built_by");

            await Task.Yield();

            return (ActionResult)new OkObjectResult(new
            {
                InvocationId = _invocationId,
                Application = "Mako Functions",
                Message = "Ping Response",
                AppVersion = appVersion,
                BuildRun = buildRun,
                BuildCommitHash = buildCommitHash,
                BuildBranch = buildBranch,
                BuildDate = buildDate,
                InvocationDate = DateTimeOffset.UtcNow
            });
        }

        private static IConfigurationRoot GetConfig(ExecutionContext context)
        {
            return new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("version.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}

