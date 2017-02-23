using EventRegistrationServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistrationWorkerRole
{
    class EventRegistratorImplementation : IEventRegistrator
    {
        public string[] GetTags()
        {
            return new string[] {"Мячик", "Котик", "Пенополиуретан"};
        }

        public void Register(string hashTag, object place)
        {
        }

        public void Register(string hashTag, object place, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
