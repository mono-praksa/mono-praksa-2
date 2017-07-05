using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;

namespace GeoEvents.Model
{
    class Image : IImage
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public byte[] Content { get; set; }
    }
}
