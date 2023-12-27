using System.Collections.Generic;

namespace WebShare.Models
{
    public class BlogViewModel
    {
        public List<Post> Posts { get; set; }
        public List<LikedUser> TopLikers { get; set; }
    }
}
