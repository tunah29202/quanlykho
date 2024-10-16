using System;

namespace Common
{
    public interface IEntity
    {
        Guid id { get; set; }
        bool del_flg { get; set; }
        DateTime created_at { get; set; }
        Guid created_by { get; set; }
        DateTime? updated_at { get; set; }
        Guid? updated_by { get; set; }
    }
}
