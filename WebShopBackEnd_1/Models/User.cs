using System;
using System.Collections.Generic;

namespace WebShopBackEnd_1.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime LastLogin { get; set; }

    public int PermissionId { get; set; }
}
