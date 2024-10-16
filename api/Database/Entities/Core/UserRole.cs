using System;
using Database.Common;

namespace Database.Entities
{
    public partial class UserRole : BaseEntity
    {
        public Guid user_id { get; set; }
        public User user { get; set; }
        public string role_cd { get; set; }
        public Role role{ get; set; }
        public UserRole()
        {
            id = Guid.NewGuid();
        }
    }
}