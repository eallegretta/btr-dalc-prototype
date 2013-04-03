using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogTalkRadio.Common.Data;
using Cinchcast.Framework.Commands;
using Web.Backend.Data;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend.Services.Commands
{
    public abstract class RepositoryCommand<T>: Command<T> where T: class, new()
    {
        public IRepository<T> Repository { protected get; set; }
    }
}