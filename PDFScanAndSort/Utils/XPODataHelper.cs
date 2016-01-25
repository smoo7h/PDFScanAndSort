using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using GIBS.Module.Models.Staff;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Threading;
using DevExpress.XtraEditors;

namespace PDFScanAndSort.Utils
{
    public class XPODataHelper
    {

        public ServerApplication serverApplication;

        public void DatabaseQuery(string searchQuery)
        {
            IObjectSpace space = this.Connect();

            using (space)
            {




            }

        }

        public void Disconnect()
        {
            serverApplication.LogOff();
            serverApplication.Dispose();
        }

        public IObjectSpace Connect()
        {
            try
            {
                serverApplication = new ServerApplication();

                serverApplication.ApplicationName = "GIBS";

                // The service can only manage workflows for those business classes that are contained in Modules specified by the serverApplication.Modules collection.
                // So, do not forget to add the required Modules to this collection via the serverApplication.Modules.Add method.
                serverApplication.Modules.Add(new SecurityModule());
                //  serverApplication.Modules.Add(new WorkflowModule());
                serverApplication.Modules.Add(new GIBS.Module.GIBSModule());

                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                {
                    serverApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                }

                Console.WriteLine("Logging into GIBS");

                serverApplication.Security = new SecurityStrategyComplex(typeof(Employee), typeof(Group), new ServerAuthentication(new BinaryOperator("UserName", "WorkflowService")));
                serverApplication.Setup();
                serverApplication.Logon();


                IObjectSpaceProvider objectSpaceProvider = serverApplication.ObjectSpaceProvider;

                IObjectSpace space = objectSpaceProvider.CreateObjectSpace();

                return space;

            }
            catch (Exception ex )
            {
                
                return null;
            }
        }

        public void SaveFileToDataBase(string ObjectKey, string fileLocation)
        {
            IObjectSpace space = this.Connect();

            using (space)
            {
           //     Program p = space.GetObjectByKey<Program>(new Guid(ObjectKey));



            }

            this.Disconnect();

        }


    }
}
