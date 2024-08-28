﻿namespace FoodService.Data.Models.Dtos;

public class FoodDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Describe { get; set; }
    public decimal Price { get; set; }
}
