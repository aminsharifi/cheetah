﻿using Cheetah_Business.Data;
using Cheetah_Business.Dimentions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace Cheetah_Business.Links;

[Table(nameof(L_RolePosition), Schema = nameof(TableType.Links))]
[Index(nameof(Name), IsUnique = true, AllDescending = true)]
[Index(nameof(CreateTimeRecord), IsUnique = true, AllDescending = true)]
[Index(nameof(LastUpdatedRecord), IsUnique = true, AllDescending = true)]
[Index(nameof(ERPCode), IsUnique = false, AllDescending = true)]
[Index(nameof(EnableRecord), IsUnique = false, AllDescending = true)]
[Index(nameof(FirstId), IsUnique = false, AllDescending = true)]
[Index(nameof(SecondId), IsUnique = false, AllDescending = true)]
public partial class L_RolePosition : SimpleLinkClass
{
    [Column(Order = 100)]
    [ForeignKey(nameof(FirstId))]
    public virtual D_Role? D_Role { get; set; }
    [Column(Order = 101)]
    [ForeignKey(nameof(SecondId))]
    public virtual D_Position? Position { get; set; }

    public L_RolePosition ShallowCopy()
    {
        return (L_RolePosition)this.MemberwiseClone();
    }
}