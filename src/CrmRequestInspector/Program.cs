using System;
using System.Configuration;
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
            ExecuteOperation();
        }

        private static void InitializeLog()
        {
            string logFolderPath = ConfigurationManager.AppSettings["LogFolderPath"];

            if (string.IsNullOrEmpty(logFolderPath))
            {
                logFolderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }

            var loggerConfiguration =
                new LoggerConfiguration()
                    .WriteTo.RollingFile(string.Concat(logFolderPath, @"\Log-{Date}.txt"));

            loggerConfiguration.MinimumLevel.Debug();

            Log.Logger = loggerConfiguration.CreateLogger();
        }

        private static void ExecuteOperation()
        {
            string crmConnectionString = ConfigurationManager.ConnectionStrings["crm"].ConnectionString;
            var crmConnection = new CrmConnection(new ConnectionStringSettings("Crm", crmConnectionString));

            using (var organizationService = CreateProxy(crmConnection))
            {
                var retrieveOperation = new RetrieveOperation(organizationService, Log.Logger);
                retrieveOperation.Execute();

                Console.WriteLine("Execution completed successfully.");
            }
        }


        private static OrganizationServiceProxy CreateProxy(CrmConnection connection)
        {
            var serviceProxy = new OrganizationServiceProxy(connection.ServiceUri, connection.HomeRealmUri,connection.ClientCredentials, connection.DeviceCredentials);

            var inspector = new CustomMessageInspector(Log.Logger);
            serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(inspector);

            return serviceProxy;
        }
    }
}
