using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Web.Backend.DomainModel.Contracts;

namespace Web.Backend
{
    public class UnitOfWorkHttpModule: IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += (sender, args) =>
                {
                    var b = 2;
                };
            context.AcquireRequestState += (sender, args) =>
                {
                    var a = 2;
                };

            context.BeginRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().Begin();
            context.EndRequest += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
            context.Error += (sender, args) => ServiceLocator.Current.GetInstance<IUnitOfWork>().End();
        }
    }
}