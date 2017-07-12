using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Repository.Common;
using GeoEvents.DAL;
using Npgsql;
using GeoEvents.Common;
using GeoEvents.Model.Common;
using AutoMapper;
using System.Data.Common;

namespace GeoEvents.Repository
{
    public class ImageRepository : IImageRepository
    {
        protected IPostgresConnection PostgresConn { get; private set; }
        protected IMapper Mapper { get; private set; }

        public ImageRepository(IPostgresConnection connection,IMapper mapper)
        {
            this.PostgresConn = connection;
            this.Mapper = mapper;
        }



        public async Task<IImage> CreateImageAsync(IImage img)
        {
            ImageEntity DbImage = Mapper.Map<ImageEntity>(img);
            ImageEntity selectImage = null;
            try
            {
                using (PostgresConn.NpgConn())
                using (NpgsqlCommand commandInsert = new NpgsqlCommand(ConstRepository.GetInsertStringImages(),
                         PostgresConn.NpgConn()))
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(ConstRepository.GetSelectStringImage(),
                         PostgresConn.NpgConn()))
                {
                    // // insert image
                    commandInsert.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, DbImage.Id);
                    commandInsert.Parameters.AddWithValue("@Content", NpgsqlTypes.NpgsqlDbType.Bytea, DbImage.Content);
                    commandInsert.Parameters.AddWithValue("@EventId", NpgsqlTypes.NpgsqlDbType.Uuid, DbImage.EventId);

                    await PostgresConn.connection.OpenAsync();
                    await commandInsert.ExecuteNonQueryAsync();
                    // //

                    // // Select image ,that we just inserted, from db 
                    commandSelect.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, DbImage.Id);

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
            catch(NpgsqlException ex)
            {
                throw ex;
            }
            return Mapper.Map<IImage>(selectImage);
        }


        public async Task<IEnumerable<IImage>> GetImagesAsync(Guid eventID)
        {
            List<IImage> selectImages = new List<IImage>();
            try
            {
                using (PostgresConn.connection)
                using (NpgsqlCommand command = new NpgsqlCommand(ConstRepository.GetSelectStringImages(),
                     PostgresConn.NpgConn()))
                {
                    command.Parameters.AddWithValue("@EventID", NpgsqlTypes.NpgsqlDbType.Uuid, eventID);

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



    }


}
