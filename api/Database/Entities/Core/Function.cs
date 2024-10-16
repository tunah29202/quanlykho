using System;
using Database.Common;

namespace Database.Entities
{
    public partial class Function : BaseEntity
    {
        public string? code { get; set; }
        public string name { get; set; }
        public string? url { get; set; }
        public string? method { get; set; }
        public string? parent_cd { get; set; }
        public Function? parent { get; set; }

        public virtual ICollection<Function>? children { get; set; }

        public virtual ICollection<Permission>? permissions { get; set; }
        public Function()
        {
            id = Guid.NewGuid();
        }
    }
}