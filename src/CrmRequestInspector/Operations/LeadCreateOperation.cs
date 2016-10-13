using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Serilog;

namespace CrmRequestInspector.Operations
{
    public class LeadCreateOperation : OperationBase
    {
        public LeadCreateOperation(IOrganizationService organizationService, ILogger log) : base(organizationService, log)
        {
        }

        public override void Execute()
        {
            Entity entity = new Entity("Lead");
            entity.Attributes["subject"] = "my test topic";
            this.organizationService.Create(entity);
        }
    }
}
