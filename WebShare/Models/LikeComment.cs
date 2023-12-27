using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class LikeComment
{
    public int Id { get; set; }

    public int? IdComment { get; set; }

    public int? IdUser { get; set; }
}
