using BlogTalkRadio.Common.Data.NHibernate.Queries;
using NHibernate;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.NHibernate
{
    public class AllGenresNative: NHibernateQuery<GenreEntityNHibernate>
    {
        public override IQueryOver<GenreEntityNHibernate, GenreEntityNHibernate> CreateQuery(IQueryOver<GenreEntityNHibernate, GenreEntityNHibernate> queryOver)
        {
            return queryOver;
        }
    }
}