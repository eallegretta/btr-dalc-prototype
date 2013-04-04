using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;
using Web.Backend.Data.Queries.Category;
using Web.Backend.DomainModel.Entities;

namespace Web.Backend.Data
{
    public static class DataSources
    {
        public static readonly DataSource Query = new SqlDataSource("Query");
        public static readonly DataSource Btr = new SqlDataSource("Btr");
        public static readonly DataSource Pactolous = new SqlDataSource("Pactolous");
        public static readonly DataSource Misc = new SqlDataSource("Misc");
        public static readonly DataSource Rss = new SqlDataSource("Rss");

        public static void InitializeMappings()
        {
            DataSourceMapper.Map(Query, Btr)
                .ToNamespace<GenreEntity>()
                .DefaultForReading(Query)
                .DefaultForWriting(Btr);
        }
    }
}