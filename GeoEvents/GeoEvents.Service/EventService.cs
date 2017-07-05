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
        public List<IEvent> GetEvents(Filter filter)
        {
            var events = new List<IEvent>();
            List<IEventEntity> entities = Repository.GetEvents(filter);
            foreach (IEvent entity in entities)
            {
                IEvent evt = Mapper.Map<IEvent>(entity);
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
                events.Add(evt);
            }
            return events;
        }

        /// <summary>
        /// Creates event.
        /// </summary>
        /// <returns></returns>
        public bool CreateEvent(IEvent evt)
        {
            evt.Category = 0;
            for (int i = 0; i < evt.Categories.Count; i++)
            {
                evt.Category += evt.Categories[i];
            }
            return Repository.CreateEvent(Mapper.Map<IEventEntity>(evt));
        }

        #endregion Methods

    }
}
