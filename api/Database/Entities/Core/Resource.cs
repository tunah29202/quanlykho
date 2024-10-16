using System;
using Database.Common;

namespace Database.Entities
{
    public partial class Resource : BaseEntity
    {
        public string lang { get; set; }
        public string module { get; set; }
        public string screen { get; set; }
        public string key { get; set; }
        public string text { get; set; }
        public Resource()
        {
            id = Guid.NewGuid();
        }
    }
}