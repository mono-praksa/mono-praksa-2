using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Model.Common
{
    public interface IImage
    {
        #region Properties
        Guid Id { get; set; }
        Guid EventId { get; set; }
        byte[] Content { get; set; }
        #endregion Properties
    }
}
