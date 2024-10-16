using System;
using Database.Common;

namespace Database.Entities
{
    public partial class Permission : BaseEntity
    {
        public string role_cd { get; set; }
        public Role role{ get; set; }
        public string function_cd { get; set; }
        public Function function { get; set; }
        public Permission()
        {
            id = Guid.NewGuid();
        }
    }
}