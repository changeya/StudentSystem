﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Class_system_Backstage_pj.Models;

public partial class T訂餐營業時間表
{
    public int 營業時間表id { get; set; }

    public int 店家id { get; set; }

    public string 星期 { get; set; }

    public string 時段早中晚全 { get; set; }

    public string 開始營業時間 { get; set; }

    public string 結束營業時間 { get; set; }

    public virtual T訂餐店家資料表 店家 { get; set; }
}