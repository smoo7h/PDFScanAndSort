using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;

namespace PDFScanAndSort.Utils
{
    public class ServerAuthentication : AuthenticationBase
    {
        private CriteriaOperator workflowWorkerUserCriteria;

        public ServerAuthentication(CriteriaOperator workflowWorkerUserCriteria)
        {
            this.workflowWorkerUserCriteria = workflowWorkerUserCriteria;
        }

        public override object Authenticate(IObjectSpace objectSpace)
        {
            object user = objectSpace.FindObject(UserType, workflowWorkerUserCriteria);
            if (user == null)
            {
                throw new AuthenticationException("", "Cannot find workflow worker user.");
            }
            return user;
        }

        public override Type UserType { get; set; }

        public override bool AskLogonParametersViaUI
        {
            get { return false; }
        }

        public override bool IsLogoffEnabled
        {
            get { return false; }
        }
    }
}
