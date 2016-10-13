using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using CommandLine;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk.Client;
using Serilog;

namespace CrmRequestInspector
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeLog();

            var options = Parser.Default.ParseArguments<CommandlineOptions>(args);
            options.WithParsed(x => ExecuteOperation(x.Operations, x.IsResumeOnError));
        }

        private static void InitializeLog()
        {
            var loggerConfiguration =
                new LoggerConfiguration()
                    .ReadFrom.AppSettings()
                    .CreateLogger();

            Log.Logger = loggerConfiguration;
        }

        private static void ExecuteOperation(IEnumerable<string> operations, bool isResumeOnError)
        {
            if (!operations.Any())
            {
                Log.Warning("No operations specified. Exiting.");
                return;
            }

            string crmConnectionString = ConfigurationManager.ConnectionStrings["crm"].ConnectionString;
            var crmConnection = new CrmConnection(new ConnectionStringSettings("Crm", crmConnectionString));

            using (var organizationService = CreateProxy(crmConnection))
            {
                Log.Information($"{operations.Count()} operations found.");

                foreach (string op in operations)
                {
                    try
                    {
                        Log.Information($"Executing operation ({op})...");
                        var executedOperation = GetExecutionOperation(op, organizationService, Log.Logger);
                        executedOperation.Execute();
                        Log.Information($"Operation ({op}) completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Error while attempting to exectue ({op})");
                        if (!isResumeOnError)
                        {
                            throw;
                        }
                    }
                }

                Console.WriteLine("Execution complete.");
            }
        }

        private static OrganizationServiceProxy CreateProxy(CrmConnection connection)
        {
            var serviceProxy = new OrganizationServiceProxy(connection.ServiceUri, connection.HomeRealmUri, connection.ClientCredentials, connection.DeviceCredentials);

            var inspector = new CustomMessageInspector(Log.Logger);
            serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(inspector);

            return serviceProxy;
        }

        private static OperationBase GetExecutionOperation(string operationName, Microsoft.Xrm.Sdk.IOrganizationService organizationService, ILogger logger)
        {
            Type baseType = typeof(OperationBase);

            var assembly = Assembly.GetExecutingAssembly();

            var operationClass = assembly.GetTypes()
                                    .Where(baseType.IsAssignableFrom)
                                    .Where(t => baseType != t)
                                    .FirstOrDefault(t => t.Name == operationName);

            if (operationClass == null)
            {
                throw new InvalidOperationException($"Unable to find execution operation with name ({operationName}).");
            }

            return (OperationBase)Activator.CreateInstance(operationClass, organizationService, logger);
        }
    }
}
