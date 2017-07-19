using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    public class ImageRepository : IImageRepository
    {
        #region Properties

        protected IPostgresConnection PostgresConn { get; private set; }
        protected IMapper Mapper { get; private set; }

        #endregion Properties

        #region Constructors

        public ImageRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.PostgresConn = connection;
            this.Mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates Image asynchronously.
        /// </summary>
        /// <param name="img"></param>
        /// <returns>
        /// return Created image (IEvent)
        /// </returns>
        public async Task<IImage> CreateImageAsync(IImage img)
        {
            ImageEntity DbImage = Mapper.Map<ImageEntity>(img);

            ImageEntity selectImage = null;
            try

            {
                using (PostgresConn.CreateConnection())
                using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertImagesString(),
                         PostgresConn.CreateConnection()))
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectImageString(),
                         PostgresConn.CreateConnection()))
                {
                    // // insert image
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, DbImage.Id);
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParContent, NpgsqlDbType.Bytea, DbImage.Content);
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, DbImage.EventId);

                    await PostgresConn.CreateConnection().OpenAsync();
                    await commandInsert.ExecuteNonQueryAsync();
                    // //

                    // // Select image ,that we just inserted, from db
                    commandSelect.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, DbImage.Id);

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
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            return Mapper.Map<IImage>(selectImage);
        }

        /// <summary>
        /// Gets Images asynchronously.
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>
        /// list of Images.
        /// </returns>
        public async Task<IEnumerable<IImage>> GetImagesAsync(Guid eventID)
        {
            List<IImage> selectImages = new List<IImage>();
            try

            {
                using (PostgresConn.CreateConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectImagesString(),
                     PostgresConn.CreateConnection()))
                {
                    command.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventID);

                    await PostgresConn.CreateConnection().OpenAsync();
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
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            return Mapper.Map<IEnumerable<IImage>>(selectImages);
        }

        #endregion Methods
    }
}