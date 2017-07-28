using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

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
        /// Gets an event with an id.
        /// </summary>
        /// <param name="id">The id of the event.</param>
        /// <returns>The event.</returns>
        public async Task<IEvent> GetEventByIdAsync(Guid id)
        {
            IEvent evt = await Repository.GetEventByIdAsync(id);
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
            int repeaton = evt.RepeatOn;
            mult = 1;
            if (repeaton != 0)
            {
                while (repeaton > 0)
                {
                    int mod = repeaton % 2;
                    if (mod == 1)
                    {
                        evt.RepeatOnList.Add(mult);
                    }
                    mult *= 2;
                    repeaton = repeaton >> 1;
                }
            }
            return evt;
        }

        /// <summary>
        /// Gets events that satisfy a filter asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// List of events.
        /// </returns>
        public async Task<StaticPagedList<IEvent>> GetEventsAsync(IFilter filter)
        {
            StaticPagedList<IEvent> result = await Repository.GetEventsAsync(filter);


            foreach (IEvent evt in result)
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
                int repeaton = evt.RepeatOn;
                mult = 1;
                if (repeaton != 0)
                {
                    while (repeaton > 0)
                    {
                        int mod = repeaton % 2;
                        if (mod == 1)
                        {
                            evt.RepeatOnList.Add(mult);
                        }
                        mult *= 2;
                        repeaton = repeaton >> 1;
                    }
                }
            }


            return result;
        }



        /// <summary>
        /// Creates an event asynchronously.
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
            evt.Reserved = 0;
            evt.Rating = 0;
            evt.RateCount = 0;
            evt.RepeatOn = 0;
            if (evt.Occurrence == "none")
            {
                evt.RepeatEvery = 0;
                evt.RepeatCount = 0;
            }
            else
            {
                for (int i = 0; i < evt.RepeatOnList.Count; i++)
                {
                    evt.RepeatOn += evt.RepeatOnList[i];
                }
            }

            return Repository.CreateEventAsync(evt);
        }

        /// <summary>
        /// Updates the reservation asynchronously.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public Task<IEvent> UpdateReservationAsync(Guid eventId)
        {
            return Repository.UpdateReservationAsync(eventId);
        }

        /// <summary>
        /// Updates the rating asynchronously.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public Task<IEvent> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount)
        {
            return Repository.UpdateRatingAsync(eventId, rating, CurrentRating, RateCount);
        }

        #endregion Methods
    }
}