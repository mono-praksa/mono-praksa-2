using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IImageEntity
    {
        Guid Id { get; set; }
        Guid EventId { get; set; }
        byte[] Content { get; set; }
    }
}

