using Ucoin.Framework.ObjectMapper;

namespace Ucoin.Framework.Service
{
    public static class MappingExtensions
    {
        public static TModel ToModel<TEntity, TModel>(this object entity)
            where TModel : class
            where TEntity : class
        {
            return Mapper.Map<TEntity, TModel>(entity as TEntity);
        }

        public static TEntity ToEntity<TModel, TEntity>(this object model)
            where TModel : class
            where TEntity : class
        {
            return Mapper.Map<TModel, TEntity>(model as TModel);
        }
    }
}
