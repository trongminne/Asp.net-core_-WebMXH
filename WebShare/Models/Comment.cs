using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? IdUser { get; set; }

    public int? IdPost { get; set; }

    public string? Contents { get; set; }

    public string? Img { get; set; }

    public DateTime? DatePost { get; set; }

    public virtual Post? IdPostNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
