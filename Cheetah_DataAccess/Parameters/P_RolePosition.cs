﻿namespace Cheetah_DataAccess.Parameters
{
    using Cheetah_DataAccess.Data;
    using Cheetah_DataAccess.Masters;
    using Cheetah_DataAccess.Systems;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("P_RolePosition", Schema = "Parameters")]
    public partial class P_RolePosition : BasePSClass
    {
        public virtual S_Role? UP_Role { get; set; }
        public virtual P_PositionOrg? UP_PositionOrg { get; set; }
    }
}
