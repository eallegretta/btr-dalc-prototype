#BTR DataAccess proposal prototype

The following project contains the base code for the repository pattern implemented using NHibernate and EntityFramework

The class responsible for performing the data access queries is the IRepository<T> interface defined as follows

    public interface IRepository<T> where T: class, new()
    {
        T Get(object id);
        T Get(string field, object value);
        T Get(Expression<Func<T, object>> property, object value);
        T Get(IQuery<T> query);
        int Count(IQuery<T> query = null);
        List<T> GetAll(int skip = 0, int take = 1000);
        IQueryable<T> Query();
        T SaveOrUpdate(T instance);
        void Delete(object id);
        void DeleteAll();
    }

Queries should be performed by using que Query method in convination with the extension method Where that supports the IQuery<T> interface, this interface should be implemented by any class that will need to send filtering information to a service method.

The IQuery<T> interface is defined as follows

    public interface IQuery<T>
    {
        Expression<Func<T, bool>> MatchingCriteria { get; }
    }

The following is a simple expample of a query that is used to get all the category topics filtered by a specific category

    public class CategoryTopicsByCategory : IQuery<CategoryTopicEntity>
    {
        public CategoryTopicsByCategory(string categoryUrl)
        {
            MatchingCriteria = x => x.Category.GenreUrl == categoryUrl;
        }

        public Expression<Func<CategoryTopicEntity, bool>> MatchingCriteria { get; private set; }
    }

### Building the application

In order to build the application you need to first specify the framework to access the database, both framework cannot coexists so the set-orm.ps1 file must be execute to determine is you want to test the prototype using NHibernate or EntityFramework

    set-orm nh will prepare the project for NHibernate usage
    set-orm ef will prepare the project for EntityFramework usage

### Profiling the queries

As part of the project is also bundled the EFProf and NHProf, both tools need a trial license that you can get from http://www.hibernatingrhinos.com/

The path to run the NHProf is 

    packages\NHibernateProfiler.2.0.2143.0\tools\EFProf.exe
The path to run the EFProf is 

    packages\EntityFrameworkProfiler.2.0.2143.0\tools\EFProf.exe


Happy testing and PROGRAMMING MOTHAFUCKA!!!!
