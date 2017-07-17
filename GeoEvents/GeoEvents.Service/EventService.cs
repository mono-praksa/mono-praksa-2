using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <value>
        /// The repository.
        /// </value>
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
        /// Gets events that satisfy a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// List of events.
        /// </returns>
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
        /// Gets the number of events that satisfy a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The number.
        /// </returns>
        public Task<Int64> GetEventCountAsync(IFilter filter)
        {
            return Repository.GetEventCountAsync(filter);
        }

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="evt">The event.</param>
        /// <returns>
        /// The event that was created.
        /// </returns>
        public Task<IEvent> CreateEventAsync(IEvent evt)
        {
            evt.Id = Guid.NewGuid();
            evt.Category = 0;
            for (int i = 0; i < evt.Categories.Count; i++)
            {
                evt.Category += evt.Categories[i];
            }
            return Repository.CreateEventAsync(evt);
        }

        #endregion Methods
    }
}