using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data.Queries;
using Web.Backend.DomainModel.Entities.OrmVersus;

namespace Web.Backend.Data.Queries.OrmVersus.Dapper
{
    public class AllGenresSp: StoredProcedureQuery<GenreEntityDapper>
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