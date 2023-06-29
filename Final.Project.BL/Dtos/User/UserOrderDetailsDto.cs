﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Project.BL;

public class UserOrderDetailsDto
{
    public string title { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;
    public int Quantity { get; set;} = 0;
    public decimal Price { get; set; }


}