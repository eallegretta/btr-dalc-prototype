# BTR Data Access Proposal Prototype

The following prototype is a simple prototype that shows how to use NHibernate and EntityFramework with the Repository pattern.

Before compiling the solution you need to specify which framework you want to use and in order to do that you need to execute the set-orm.ps1 file

set-orm "nh" sets the project in NHibernate mode
set-orm "ef" sets the project in EntityFramework mode

This commands will reference the correct profiler Dll and will set to None or Compile the corresponding NHibernate or EntityFramework autofac modules

The application uses the UnitOfWorkHttpModule to perform a one connection per session pattern and this module is added using the DynamicModuleUtility.RegisterModule that is located in the HttpModules file inside the App_Start

 