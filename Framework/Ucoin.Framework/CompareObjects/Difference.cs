using System;

namespace Ucoin.Framework.CompareObjects
{
    public class Difference
    {
        public string PropertyName { get; set; }

        public string Object1Value { get; set; }

        public string Object2Value { get; set; }

        public string ObjectTypeName { get; set; }

        public WeakReference Object1 { get; set; }       

        public override string ToString()
        {
            var msg = GetWhatIsCompared();
            if (string.IsNullOrEmpty(msg))
            {
                return String.Format("Values [{0},{1}]", Object1Value, Object2Value);
            }
            return String.Format("{0}, Values [{1},{2}]", msg, Object1Value, Object2Value);
        }

        private string GetWhatIsCompared()
        {
            var message = string.Empty;

            if (!String.IsNullOrEmpty(PropertyName))
            {
                message = String.Format("Type:[{0}], Property:[{1}]", ObjectTypeName, PropertyName);
            }

            return message;
        }
    }
}
