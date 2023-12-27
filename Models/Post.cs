using System;
using System.Collections.Generic;

namespace WebShare.Models;

public partial class Post
{
    public int Id { get; set; }

    public string? Contents { get; set; }

    public int? IdUser { get; set; }

    public string? Filename { get; set; }
    public DateTime? DatePost { get; set; }

    public int? IdSub { get; set; }

    public virtual Subject? IdSubNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public int CommentCount => Comments.Count;

    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    public int LikeCount => Likes.Count;
}
