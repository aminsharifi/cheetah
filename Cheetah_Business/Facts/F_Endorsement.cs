﻿using Cheetah_Business.Data;
using Cheetah_Business.Dimentions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace Cheetah_Business.Facts
{
    [Table(nameof(F_Endorsement), Schema = nameof(TableType.Facts))]
    [Index(nameof(PCode), IsUnique = true, AllDescending = true)]
    [Index(nameof(PIndex), IsUnique = true, AllDescending = true)]
    [Index(nameof(PName), IsUnique = true, AllDescending = true)]
    [Index(nameof(CreateTimeRecord), IsUnique = true, AllDescending = true)]
    [Index(nameof(LastUpdatedRecord), IsUnique = true, AllDescending = true)]
    [Index(nameof(PERPCode), IsUnique = false, AllDescending = true)]
    [Index(nameof(DsblRecord), IsUnique = false, AllDescending = true)]
    [Index(nameof(Parent_Id), IsUnique = false, AllDescending = true)]
    public partial class F_Endorsement : BaseClass<F_Endorsement>
    {
        [Column(Order = 100)]
        public long? ED_RoleId { get; set; }
        public virtual D_Role? ED_Role { get; set; }

        [Column(Order = 101)]
        public long? ED_ScenarioId { get; set; }
        public virtual F_Scenario? ED_Scenario { get; set; }

        public virtual ICollection<F_Condition>? ED_Conditions { get; set; } = new HashSet<F_Condition>();

        public override void SetName()
        {
            PDisplayName = ED_Scenario?.PDisplayName + "," + ED_Role?.PDisplayName;
            PName = ED_Scenario?.PName + "," + ED_Role?.PName;
        }
    }
}
