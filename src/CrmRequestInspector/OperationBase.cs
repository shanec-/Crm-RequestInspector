using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Serilog;
using System.Configuration;

namespace CrmRequestInspector
{
    public abstract class OperationBase
    {
        protected ILogger log;
        protected IOrganizationService organizationService;

        public OperationBase(IOrganizationService organizationService, ILogger log)
        {
            this.log = log;
            this.organizationService = organizationService;
        }

        public abstract void Execute();
    }
}
