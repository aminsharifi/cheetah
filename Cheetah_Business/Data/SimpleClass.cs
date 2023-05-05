﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cheetah_Business.Data
{
    public abstract class SimpleClass
    {
        [NotMapped]
        public String? TableName { get; set; }

        [Key]
        [Description("شناسه")]
        [Column(Order = 0)]
        public Int64 Id { get; set; }

        [Description("کد")]
        [Required(ErrorMessage = "کد اجباری است")]
        [Column(Order = 1)]
        public Int64 PCode { get; set; }

        [Description("اندیس مرتب‌سازی")]
        [Column(Order = 2)]
        [Required(ErrorMessage = "اندیس سورت اجباری است")]
        public Int64? PIndex { get; set; }


        [StringLength(512)]
        [Description("نام")]
        [Column(Order = 3)]
        [Required(ErrorMessage = "نام اجباری است")]
        public String? PName { get; set; }

        [StringLength(512)]
        [Description("نام نمایشی")]
        [Required(ErrorMessage = "نام نمایشی اجباری است")]
        [Column(Order = 4)]
        public String? PDisplayName { get; set; }


        [StringLength(512)]
        [Description("توضیحات")]
        [Column(Order = 5)]
        public String? PDescription { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 6)]
        public DateTime? CreateTimeRecord { get; set; } = DateTime.Now;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(Order = 7)]
        [ConcurrencyCheck]
        public DateTime? LastUpdatedRecord { get; set; } = DateTime.Now;

        [Column(Order = 8)]
        public Guid? GuidRecord { get; set; } = Guid.NewGuid();
        [DefaultValue(false)]
        [Column(Order = 9)]
        [Description("غیرفعال")]
        public bool DsblRecord { get; set; } = false;

        [Description("کد در ERP")]
        [Column(Order = 10)]
        public Int64? PERPCode { get; set; }

        public virtual void SetName()
        {

        }
    }
}
