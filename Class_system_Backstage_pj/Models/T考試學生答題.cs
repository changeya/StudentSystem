﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T考試學生答題
{
    public int Id { get; set; }

    public int? 學生id { get; set; }

    public int? 考試id { get; set; }

    public string 答案 { get; set; }

    public DateTime? 提交時間 { get; set; }

    public virtual T會員學生 學生 { get; set; }

    public virtual T考試考試總表 考試 { get; set; }
}