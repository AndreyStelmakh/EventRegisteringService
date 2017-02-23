using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace EventRegistrationServiceContracts
{
    [ServiceContract]
    public interface IEventRegistrator
    {
        [OperationContract]
        void Register(string hashTag, object place, DateTime time);
        [OperationContract]
        void Register(string hashTag, object place);
        [OperationContract]
        string[] GetTags();
    }
}
