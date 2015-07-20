
namespace Ucoin.Identity.DataObjects
{
    public class ActionInfo
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }

        public int ActionId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string IconClass { get; set; }

        public string Sequence { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public string Url { get; set; }

        public bool AccessControl { get; set; }

        public bool Selected { get; set; }
    }
}
