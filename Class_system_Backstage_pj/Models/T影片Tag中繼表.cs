﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T影片Tag中繼表
{
    public int FId { get; set; }

    public int FVideoId { get; set; }

    public int FTagId { get; set; }

    public virtual T影片Tag表 FTag { get; set; }

    public virtual T影片Video FVideo { get; set; }
}