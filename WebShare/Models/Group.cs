using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Group
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Avatar { get; set; }

    public string? CoverImage { get; set; }

    public string? Rules { get; set; }
}
