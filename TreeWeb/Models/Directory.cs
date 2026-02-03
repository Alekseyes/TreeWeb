using Microsoft.EntityFrameworkCore;
using static TreeWeb.AppContext.TreeWebDbContext;

namespace TreeWeb.Models
{
    [EntityTypeConfiguration(typeof(DirectoryConfiguration))]
    public class Directory
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public long? ParentId { get; set; }
        public Directory? Parent { get; set; }
        public ICollection<Directory> Children { get; set; } = new List<Directory>();
    }
}
