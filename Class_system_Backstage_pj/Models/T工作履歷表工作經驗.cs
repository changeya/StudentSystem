﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T工作履歷表工作經驗
{
    public int FId { get; set; }

    public int F履歷Id { get; set; }

    public int F工作經驗Id { get; set; }

    public virtual T工作履歷資料 F履歷 { get; set; }

    public virtual T工作工作經驗 F工作經驗 { get; set; }
}