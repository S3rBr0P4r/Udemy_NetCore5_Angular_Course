using System.ComponentModel.DataAnnotations.Schema;

namespace Udemy.NetCore5.Angular.Data.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool Enabled { get; set; }

        public string PublicId { get; set; }

        public AppUser AppUser { get; set; }

        public int AppUserId { get; set; }
    }
}