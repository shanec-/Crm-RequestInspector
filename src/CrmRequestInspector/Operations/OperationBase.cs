using Microsoft.Xrm.Sdk;
using Serilog;

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
