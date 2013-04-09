using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data;
using BlogTalkRadio.Common.Data.Dapper;
using BlogTalkRadio.Common.Data.DataSources;
using BlogTalkRadio.Common.Data.FluentMapping;
using BlogTalkRadio.Common.Data.NHibernate;
using BlogTalkRadio.Common.Data.Orm.EntityFramework;
using Web.Backend.Data.Queries.Category;
using Web.Backend.DomainModel.Entities;
using Web.Backend.DomainModel.Entities.OrmVersus;

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
                .DefaultForWriting(Btr)
                .UsingRepository(typeof(NHibernateRepository<>));


            DataSourceMapper.Map(Query)
                            .ToType<GenreEntityDapper>()
                            .Default(Query)
                            .UsingRepository(typeof (DapperRepository<>));

            DataSourceMapper.Map(Query)
                            .ToType<GenreEntityNHibernate>()
                            .Default(Query)
                            .UsingRepository(typeof(NHibernateRepository<>));

            DataSourceMapper.Map(Query)
                            .ToType<GenreEntityEntityFramework>()
                            .Default(Query)
                            .UsingRepository(typeof(EntityFrameworkRepository<>));

        }
    }
}