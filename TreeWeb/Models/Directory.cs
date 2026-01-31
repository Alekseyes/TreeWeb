namespace TreeWeb.Models
{
    public class Directory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Directory Parent { get; set; }
        public ICollection<Directory> Children { get; set; } = new List<Directory>();
    }
}
