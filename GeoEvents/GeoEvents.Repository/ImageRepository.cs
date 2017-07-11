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

        public ImageRepository(IPostgresConnection connection)
        {
            this.PostgresConn = connection;
        }



        public bool CreateImages(List<IImage> img)
        {
            PostgresConn.OpenConnection();

            NpgsqlCommand command = PostgresConn.NpgComm();
            foreach (var item in img)
            {
                command = new NpgsqlCommand
                    (ConstRepository.GetInsertStringImages(),
                    PostgresConn.NpgConn());

                command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, item.Id);
                command.Parameters.AddWithValue("@Content", NpgsqlTypes.NpgsqlDbType.Bytea ,item.Content);
                command.Parameters.AddWithValue("@EventId", NpgsqlTypes.NpgsqlDbType.Uuid, item.EventId);
            }

            if (command.ExecuteNonQuery() == 1)
            {
                Flag = true;
            }

            PostgresConn.CloseConnection();

            return Flag;
        }


        public List<IImage> GetImages(Guid eventID)
        {
            PostgresConn.OpenConnection();

            NpgsqlCommand command = new NpgsqlCommand
                (ConstRepository.GetSelectStringImages(eventID),
                PostgresConn.NpgConn());
            command.Parameters.AddWithValue("@eventID", NpgsqlTypes.NpgsqlDbType.Uuid, eventID);

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
