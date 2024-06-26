﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T考試考試總表
{
    public int 考試id { get; set; }

    public string 考試名稱 { get; set; }

    public string 考試類型 { get; set; }

    public string 描述 { get; set; }

    public string 備註 { get; set; }

    public int? 試卷id { get; set; }

    public int? 班級id { get; set; }

    public DateTime? 開始時間 { get; set; }

    public DateTime? 結束時間 { get; set; }

    public int? 發布者 { get; set; }

    public int? 提交次數 { get; set; }

    public virtual ICollection<T考試學生成績> T考試學生成績s { get; set; } = new List<T考試學生成績>();

    public virtual ICollection<T考試學生答題> T考試學生答題s { get; set; } = new List<T考試學生答題>();

    public virtual ICollection<T考試考試權限> T考試考試權限s { get; set; } = new List<T考試考試權限>();

    public virtual T課程班級科目 班級 { get; set; }

    public virtual T會員老師 發布者Navigation { get; set; }

    public virtual T考試試題總表 試卷 { get; set; }
}