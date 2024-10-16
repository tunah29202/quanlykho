using System;
using Database.Common;

namespace Database.Entities
{
    public partial class Role : BaseEntity
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public virtual ICollection<UserRole>? user_roles { get; set; }
        public virtual ICollection<Permission> permissions { get; set; }         
        public Role()
        {
            id = Guid.NewGuid();
            this.permissions = new HashSet<Permission>();
        }
    }
}