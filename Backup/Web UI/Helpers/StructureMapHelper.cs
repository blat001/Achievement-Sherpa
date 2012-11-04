using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration;
using StructureMap.Pipeline;
using AchievementSherpa.Business;
using AchievementSherpa.Data.MongoDb;

namespace Web_UI.Helpers
{
    
    /*
    public class DomainRegistry : Registry
    {
        protected override void configure()
        {
            ForRequestedType<ICategoryService>()
               .TheDefaultIsConcreteType<CategoryService>();
            ForRequestedType<ICategoryRepository>()
               .TheDefaultIsConcreteType<CategoryRepository>();
        }
    }
     * */

    public class DBServiceRegistry : Registry
    {
        public DBServiceRegistry()
        {
            For<IWorldRepository>()
             .Use<MongoWorldRepository>();
        }
    }
}