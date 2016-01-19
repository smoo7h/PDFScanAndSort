using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;

namespace PDFScanAndSort.Utils
{
    public class ServerApplication : XafApplication
    {
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection);
        }
        protected override DevExpress.ExpressApp.Layout.LayoutManager CreateLayoutManagerCore(bool simple)
        {
            throw new NotImplementedException();
        }
        public void Logon()
        {
            base.Logon(null);
        }
        protected override void CheckCompatibilityCore()
        {
            //base.CheckCompatibilityCore();
        }

        protected override void OnDatabaseVersionMismatch(DatabaseVersionMismatchEventArgs args)
        {
            args.Handled = true;
            base.OnDatabaseVersionMismatch(args);
        }


    }
}
