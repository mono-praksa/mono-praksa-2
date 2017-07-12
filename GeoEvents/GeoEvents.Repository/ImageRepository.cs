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

namespace GeoEvents.Repository
{
    public class ImageRepository : IImageRepository
    {
        bool Flag = false;

        protected IPostgresConnection PostgresConn { get; private set; }
        protected IMapper Mapper { get; private set; }

        public ImageRepository(IPostgresConnection connection,IMapper mapper)
        {
            this.PostgresConn = connection;
            this.Mapper = mapper;
        }



        public async Task<IImage> CreateImages(IImage img)
        {
            ImageEntity DbImage = Mapper.Map<ImageEntity>(img);

            using (PostgresConn.NpgConn())
            using (NpgsqlCommand commandInsert = new NpgsqlCommand
                     (ConstRepository.GetInsertStringImages(),
                     PostgresConn.NpgConn()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand
                (ConstRepository.GetSelectStringImages(),
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

                NpgsqlDataReader dr = commandSelect.ExecuteReader();
                ImageEntity selectImage = null;

                while (dr.Read())
                {
                    selectImage = new ImageEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        EventId = DbImage.EventId
                    };

                    selectImage.Content = (byte[])dr["Content"];
                }
                // //

                return Mapper.Map<IImage>(selectImage);
            }


        }


        public async Task<IEnumerable<IImage>> GetImagesAsync(Guid eventID)
        {
            PostgresConn.OpenConnection();

            NpgsqlCommand command = new NpgsqlCommand
                (ConstRepository.GetSelectStringImages(),
                PostgresConn.NpgConn());

            command.Parameters.AddWithValue("@EventID", NpgsqlTypes.NpgsqlDbType.Uuid, eventID);

            NpgsqlDataReader dr = command.ExecuteReader();


            List<IImage> selectImages = new List<IImage>();

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

            return selectImages;
        }


    }
}
