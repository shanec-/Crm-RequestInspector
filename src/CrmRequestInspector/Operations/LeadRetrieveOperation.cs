using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Serilog;

namespace CrmRequestInspector.Operations
{
    public class LeadRetrieveOperation : OperationBase
    {
        public LeadRetrieveOperation(IOrganizationService organizationService, ILogger log) : base(organizationService, log)
        {
        }

        public override void Execute()
        {
            QueryExpression expression = new QueryExpression("lead");
            expression.ColumnSet = new ColumnSet("leadid", "subject", "statuscode");
            expression.Criteria = new FilterExpression();
            expression.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 1);

            var result = organizationService.RetrieveMultiple(expression);

            log.Debug("Result {@result}", result);
        }
    }
}
