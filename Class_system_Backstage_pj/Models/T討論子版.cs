﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T討論子版
{
    public int 子版id { get; set; }

    public string 名稱 { get; set; }

    public int? 看板id { get; set; }

    public virtual ICollection<T討論文章> T討論文章s { get; set; } = new List<T討論文章>();

    public virtual ICollection<T討論留言> T討論留言s { get; set; } = new List<T討論留言>();

    public virtual T討論看板 看板 { get; set; }
}