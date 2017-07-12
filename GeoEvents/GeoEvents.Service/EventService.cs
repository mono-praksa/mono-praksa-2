using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Service.Common;
using GeoEvents.Model.Common;
using GeoEvents.Common;
using GeoEvents.Repository.Common;
using AutoMapper;

namespace GeoEvents.Service
{
    /// <summary>
    /// The event service.
    /// </summary>
    /// <seealso cref="IEventService"/>
    public class EventService : IEventService
    {
        #region Properties
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>The repository.</value>
        protected IEventRepository Repository { get; private set; }

        #endregion Properties



        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class. 
        /// </summary>
        /// <param name="repository">The repository.</param>
        public EventService(IEventRepository repository)
        {
            this.Repository = repository;
        }


        #endregion Constructors



        #region Methods

        /// <summary>
        /// Gets events that satisfy the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public async Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter)
        {
            IEnumerable<IEvent> events = await Repository.GetEventsAsync(filter);

            foreach (IEvent evt in events)
            {
                int mult = 1;
                evt.Categories = new List<int>();
                int cat = evt.Category;
                while (cat > 0)
                {
                    int mod = cat % 2;
                    if (mod == 1)
                    {
                        evt.Categories.Add(mult);
                    }
                    mult *= 2;
                    cat = cat >> 1;
                }
            }

            return events;
        }

        /// <summary>
        /// Creates event.
        /// </summary>
        /// <returns></returns>
        public async Task<IEvent> CreateEventAsync(IEvent evt)
        {
            evt.Id = Guid.NewGuid();
            evt.Category = 0;
            for (int i = 0; i < evt.Categories.Count; i++)
            {
                evt.Category += evt.Categories[i];
            }
            return await Repository.CreateEventAsync(evt);
        }

        #endregion Methods

    }
}
