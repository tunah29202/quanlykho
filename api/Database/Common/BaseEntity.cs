using Services.Common.Repository;
using System;
using Common;

namespace Database.Common
{
    public abstract class BaseEntity : IEntity
    {
        public Guid id { get; set; }
        public DateTime created_at { get; set; }
        public Guid created_by { get; set; }
        public DateTime? updated_at { get; set; }
        public Guid? updated_by { get; set; }
        public bool del_flg { get; set; }
    }
}