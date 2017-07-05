using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Repository.Common;

namespace GeoEvents.DAL
{
    public class ImageEntity : IImageEntity
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; }

        public Guid EventId { get; set; }

    }
}
