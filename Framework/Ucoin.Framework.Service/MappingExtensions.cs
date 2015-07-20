using Ucoin.Framework.ObjectMapper;

namespace Ucoin.Framework.Service
{
    public static class MappingExtensions
    {
        public static TModel ToModel<TEntity, TModel>(this TEntity entity)
            where TModel : class
            where TEntity : class
        {
            return Mapper.Map<TEntity, TModel>(entity);
        }

        public static TEntity ToEntity<TModel, TEntity>(this TModel model)
            where TModel : class
            where TEntity : class
        {
            return Mapper.Map<TModel, TEntity>(model);
        }

        public static void Map<TSource, TTarget>(this TSource source, TTarget target)
        {
            Mapper.Map(source, target);
        }
    }
}
