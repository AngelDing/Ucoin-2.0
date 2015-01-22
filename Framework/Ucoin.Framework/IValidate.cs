
namespace Ucoin.Framework
{
    /// <summary>
    /// 如果是採用EntityFramework，則可採用IValidatableObject接口，統一在Commit判斷，
    /// 否則需要實現此接口，例如在MongoEntity中。
    /// </summary>
    public interface IValidate
    {
        void Validate();
    }
}
