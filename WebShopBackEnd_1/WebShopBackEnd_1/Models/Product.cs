using System;
using System.Collections.Generic;

namespace WebShopBackEnd_1.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public int Category { get; set; }

    public byte[] Image { get; set; } = null!;
}
