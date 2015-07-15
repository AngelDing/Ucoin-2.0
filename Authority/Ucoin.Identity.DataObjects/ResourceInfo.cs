
namespace Ucoin.Identity.DataObjects
{
    public class ResourceInfo
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string IconClass { get; set; }

        public string Url { get; set; }

        public bool IsVisible { get; set; }

        public bool IsEnable { get; set; }

        public string Sequence { get; set; }
    }
}
