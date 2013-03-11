using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Web.Backend.Contracts;

namespace Web.Backend
{
    public class UnitOfWorkHttpModule: IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().Begin();
            context.EndRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
            context.Error += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
        }
    }
}