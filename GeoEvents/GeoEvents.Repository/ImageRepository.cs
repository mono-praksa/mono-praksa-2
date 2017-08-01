using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using log4net;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;


namespace GeoEvents.Repository
{
    public class ImageRepository : IImageRepository
    {
        #region Properties

        protected IPostgresConnection Connection { get; private set; }
        protected IMapper Mapper { get; private set; }
        private static readonly ILog _log = LogManager.GetLogger(typeof(EventRepository));
        #endregion Properties

        #region Constructors

        public ImageRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.Connection = connection;
            this.Mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates an image for an event asynchronously.
        /// </summary>
        /// <param name="img">Image that will be created</param>
        /// <returns>
        /// Returns the Created image
        /// </returns>
        public async Task<IImage> CreateImageAsync(IImage img)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                ImageEntity DbImage = Mapper.Map<ImageEntity>(img);
                ImageEntity selectImage = null;

                using (var connection = Connection.CreateConnection())
                {
                    using (NpgsqlCommand commandSelect = new NpgsqlCommand(ImageQueryHelper.GetSelectImageQueryString(), connection))
                    {
                        using (NpgsqlCommand commandInsert = new NpgsqlCommand(ImageQueryHelper.GetInsertImagesQueryString(), connection))
                        {
                            commandInsert.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, DbImage.Id);
                            commandInsert.Parameters.AddWithValue(QueryConstants.ParContent, NpgsqlDbType.Bytea, DbImage.Content);
                            commandInsert.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, DbImage.EventId);

                            await connection.OpenAsync();
                            await commandInsert.ExecuteNonQueryAsync();
                        }
                        commandSelect.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, DbImage.Id);
                        DbDataReader dr = await commandSelect.ExecuteReaderAsync();

                        while (dr.Read())
                        {
                            selectImage = new ImageEntity
                            {
                                Id = new Guid(dr[0].ToString()),
                                EventId = DbImage.EventId
                            };

                            selectImage.Content = (byte[])dr["Content"];
                        }
                    }
                }
                return Mapper.Map<IImage>(selectImage);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets Images for an event asynchronously.
        /// </summary>
        /// <param name="eventID">Identifier of the event which will have it's images retrieved.</param>
        /// <returns>
        /// List of Images for an event.
        /// </returns>
        public async Task<IEnumerable<IImage>> GetImagesAsync(Guid eventID)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                List<IImage> selectImages = new List<IImage>();

                using (var connection = Connection.CreateConnection())
                {
                    using (NpgsqlCommand command = new NpgsqlCommand(ImageQueryHelper.GetSelectImagesQueryString(), connection))
                    {
                        command.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventID);

                        await connection.OpenAsync();
                        DbDataReader dr = await command.ExecuteReaderAsync();

                        while (dr.Read())
                        {
                            ImageEntity tmp = new ImageEntity
                            {
                                Id = new Guid(dr[0].ToString()),
                                EventId = eventID
                            };
                            tmp.Content = (byte[])dr["Content"];
                            selectImages.Add(Mapper.Map<IImage>(tmp));
                        }
                    }
                }
                return Mapper.Map<IEnumerable<IImage>>(selectImages);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        #endregion Methods
    }
}