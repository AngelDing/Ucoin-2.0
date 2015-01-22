
namespace Ucoin.Framework.CompareObjects
{
    public class EnumComparer : BaseTypeComparer
    {
        public EnumComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(System.Type type1, System.Type type2)
        {
            return TypeHelper.IsEnum(type1) && TypeHelper.IsEnum(type2);
        }

        public override void CompareType(CompareParms parms)
        {
            if (parms.Object1.ToString() != parms.Object2.ToString())
            {
                AddDifference(parms);
            }
        }
    }
}
