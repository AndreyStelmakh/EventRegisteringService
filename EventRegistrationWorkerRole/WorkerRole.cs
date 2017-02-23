using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
//using Microsoft.WindowsAzure.StorageClient;
using System.ServiceModel;
using EventRegistrationServiceContracts;

namespace EventRegistrationWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
        public override void Run()
        {
            Trace.TraceInformation("EventRegistrationWorkerRole is running");

            try
            {
                this.RunAsync(this._cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this._runCompleteEvent.Set();
            }
        }
        private void CreateServiceHost()
        {
            _serviceHost = new ServiceHost(typeof(EventRegistratorImplementation));

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            var externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["PublicEndpoint"];
            string endpoint = String.Format("http://{0}/LoanCalculator",
                externalEndPoint.IPEndpoint);

            _serviceHost.AddServiceEndpoint(typeof(IEventRegistrator), binding, endpoint);

            _serviceHost.Open();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("EventRegistrationWorkerRole has been started");

            return result;
        }
        public override void OnStop()
        {
            Trace.TraceInformation("EventRegistrationWorkerRole is stopping");

            this._cancellationTokenSource.Cancel();
            this._runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("EventRegistrationWorkerRole has stopped");
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private ServiceHost _serviceHost;
    }
}
