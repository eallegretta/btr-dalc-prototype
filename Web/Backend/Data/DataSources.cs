using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data
{
    public static class DataSources
    {
        public static readonly DataSource Read = new SqlDataSource("Read");
        public static readonly DataSource ReadWrite = new SqlDataSource("ReadWrite");

        public static void InitializeMappings()
        {
            DataSourceMapper.Map(Read, ReadWrite)
                .ToNamespace<GenreEntity>()
                .DefaultForReading(Read)
                .DefaultForWriting(ReadWrite);
        }
    }
}