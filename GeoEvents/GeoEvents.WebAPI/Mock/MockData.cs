using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoEvents.WebAPI.Mock
{
    public class MockData
    {
        public MockData() { }
        public List<EventsViewModel> GenerateMockData()
        {
            List<EventsViewModel> list = new List<EventsViewModel>();
            for (int i = 0; i < 10; i++)
            {
                EventsViewModel evm = new EventsViewModel();
                evm.Id = Guid.NewGuid();
                evm.Lat = 12;
                evm.Long = 10;
                evm.Name = "testing";
                evm.StartTime = DateTime.UtcNow;
                evm.EndTime = DateTime.UtcNow;
                evm.Description = "description";
                evm.Categories.Add(1);
                list.Add(evm);
            }
            return list;
        }
    }
}