using System.Collections.Generic;
using BlogTalkRadio.Common.Data.Queries;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.EntityFramework
{
    public class AllGenresSp : StoredProcedureQuery<GenreEntityEntityFramework>
    {
        public override string StoredProcedure
        {
            get { return "p_Genre_Select_All"; }
        }

        public override IDictionary<string, object> Parameters
        {
            get { return null; }
        }
    }
}