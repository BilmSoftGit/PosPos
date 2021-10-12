using System;
using System.ComponentModel;

namespace Pospos.Core.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public BaseEntity()
        {
            InsertDate = DateTime.Now;
        }

        #region DB ilişkisinde görmezden gelinecek özellikler

        [DescriptionAttribute("ignore")]
        public int TotalRowCount { get; set; }

        #endregion
    }

    public class DetailedBaseEntity : BaseEntity
    {
        public int UpdatedByUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
