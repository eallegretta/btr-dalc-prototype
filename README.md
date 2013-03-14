#BTR DataAccess proposal prototype

The following project contains the base code for the repository pattern implemented using NHibernate and EntityFramework

The class responsible for performing the data access queries is the IRepository<T> interface defined as follows

    public interface IRepository<T> where T: class, new()
    {
        T Get(object id);
        T Get(IQuery query);
        int Count(IQuery query = null);
        List<T> All(int skip = 0, int take = 1000);
        List<T> Query(IQuery query);
        T SaveOrUpdate(T instance);
        void Delete(object id);
        void DeleteAll();
    }

Queries are performed using the Query Object pattern, each query must inherit one of the base query classes that implement the IQuery interface

The IQuery interface is defined as follows

    public interface IQuery<T>
    {
    }

The following query types are defined within the system

Linq Query

    public abstract class LinqQuery<T> : IQuery
    {
        public abstract void Apply(IQueryable<T> queryable);
    }
    
Linq Paged Query

    public class LinqPagedQuery<T>: LinqQuery<T> where T: class, new()
    {
        private readonly int _skip;
        private readonly int _take;

        public LinqPagedQuery(int skip = 0, int take = 1000)
        {
            _skip = skip;
            _take = take;
        }

        public override void Apply(IQueryable<T> queryable)
        {
            if (_skip > 0)
            {
                queryable = queryable.Skip(_skip);
            }

            queryable = queryable.Take(_take);
        }
    }

Stored Procedure Query

    public abstract class StoredProcedureQuery : IQuery
    {
        public abstract string StoredProcedure { get; }
        public abstract IDictionary<string, object> Parameters { get; }
    }

The following is a simple expample of a query that is used to get all the category topics filtered by a specific category

    public class CategoryTopicsByCategory : LinqPagedQuery<CategoryTopicEntity>
    {
        private readonly string _categoryUrl;

        public CategoryTopicsByCategory(string categoryUrl, int skip = 0, int take = 1000): base(skip, take)
        {
            _categoryUrl = categoryUrl;
        }

        public override void Apply(IQueryable<CategoryTopicEntity> queryable)
        {
            queryable = queryable.Where(x => x.Category.GenreUrl == _categoryUrl);

            base.Apply(queryable);
        }
    }
    
    
Another important interface is the one that can interpret each of the different queries

    public interface IQueryInterpreter<T> where T: class, new()
    {
        bool CanInterpret(IQuery query);
        int Count(IQuery query = null);
        T Get(IQuery query);
        List<T> Query(IQuery query);
    }
    
Then its responsability of the underlying framework to create interpreter for each of the defined base query types in the system


A repository will use the query intepreter in order to execute the queries that it receives.


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
