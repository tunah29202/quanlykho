using System;
using System.ComponentModel;
using Database.Common;

namespace Database.Entities
{
    public partial class LogAction : BaseEntity
    {
        public Guid? user_id { get; set; }
        public string? user_name { get; set; }
        public string? method { get; set; }
        public string? body { get; set; }
        public string? description { get; set; }
        public User? user { get; set; }
    }
}