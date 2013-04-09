using BlogTalkRadio.Common.Data.Dapper;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Web.App_Start;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DapperStart), "Start", Order = 2)]
namespace Web.App_Start
{
    public static class DapperStart
    {
        public static void Start()
        {
            DapperExtensions.DapperExtensions.Configure(typeof(ClassMapper<>), new[] { typeof(DapperStart).Assembly}, new SqlServerDialect());
        }
    }
}