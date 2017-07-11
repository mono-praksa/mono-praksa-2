using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoEvents.DAL
{
    public class ImageEntity 
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; }

        public Guid EventId { get; set; }


        public ImageEntity(Guid Id, byte[] Content, Guid EventId)
        {
            this.Id = Id;
            this.Content = Content;
            this.EventId = EventId;
        }

        public ImageEntity()
        {

        }

    }
}
