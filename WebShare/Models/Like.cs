using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Like
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public int? IdPost { get; set; }

    public virtual Post? IdPostNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
