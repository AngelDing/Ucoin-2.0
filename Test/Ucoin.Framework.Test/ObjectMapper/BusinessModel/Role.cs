using System;

namespace Ucoin.Framework.ObjectMapper.Test
{
    public class Role
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        protected bool Equals(Role other)
        {
            return RoleId.Equals(other.RoleId) && string.Equals(RoleName, other.RoleName) &&
                   string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Role)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = RoleId.GetHashCode();
                hashCode = (hashCode * 397) ^ (RoleName != null ? RoleName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
