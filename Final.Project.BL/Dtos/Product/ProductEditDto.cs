﻿namespace Final.Project.BL;

public class ProductEditDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int CategoryID { get; set; }
    //public string StockStatus { get; set; } = string.Empty;
}
