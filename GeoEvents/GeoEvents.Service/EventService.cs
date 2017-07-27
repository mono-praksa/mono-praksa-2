﻿using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using GoogleMaps.Net.Clustering.Data.Geometry;
using GoogleMaps.Net.Clustering.Infrastructure;
using GoogleMaps.Net.Clustering.Services;
using GoogleMaps.Net.Clustering.Data.Params;

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

        #region Clustering

        /// <summary>
        /// Gets the events in the form of Map points (markers and/or clusters).
        /// Does not use caching but can be modified to cache the results from the database.
        /// </summary>
        /// <param name="filter">Filter used for retrieving events from the database.</param>
        /// <param name="clusteringFilter">Filter used for clustering the markers.</param>
        /// <returns>List of Map points.</returns>
        public async Task<IList<MapPoint>> GetClusteredEventsAsync(IFilter filter, IClusteringFIlter clusteringFilter )
        {
            var points = await GetClusterPointCollection(filter);

            var mapService = new ClusterService(points);
            var input = new GetMarkersParams()
            {
                NorthEastLatitude = clusteringFilter.NELatitude,
                NorthEastLongitude = clusteringFilter.NELongitude,
                SouthWestLatitude = clusteringFilter.SWLatitude,
                SouthWestLongitude = clusteringFilter.SWLongitude,
                ZoomLevel = clusteringFilter.ZoomLevel
            };

            var markers = mapService.GetClusterMarkers(input);

            return markers.Markers;
        }

        /// <summary>
        /// Calls the repository and gets the events from the database using the filter
        /// Then it maps these events into a pointCollection object
        /// </summary>
        /// <param name="filter">Filter that will be used to retrieve data from the database if needed</param>
        /// <returns>The PointCollection</returns>
        private async Task<PointCollection> GetClusterPointCollection(IFilter filter)
        {
            var points = new PointCollection();

            //get list of events from the database. filter is modified in a way it will recieve all pages at once.
            filter.PageNumber = -1;
            StaticPagedList<IEvent> dataBaseResult = await Repository.GetEventsAsync(filter);
            List<IEvent> dbResults = dataBaseResult.ToList();
            
            //map the location, name and id properties
            var mapPoints = dbResults.Select(p => new MapPoint() { X = p.Longitude, Y = p.Latitude, Data = p.Id, Name = p.Name }).ToList();
            
            //set the map points. caching is not used so timespan is 0...
            points.Set(mapPoints, TimeSpan.MinValue, null);

            //return the points
            return points;
        }

        #endregion
    }
}