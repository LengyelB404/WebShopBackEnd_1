using System;
using System.Collections.Generic;

namespace WebShopBackEnd_1.Models;

public partial class Token
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token1 { get; set; } = null!;

    public DateTime ExpireDate { get; set; }
}
