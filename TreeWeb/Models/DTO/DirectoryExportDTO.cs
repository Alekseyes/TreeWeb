namespace TreeWeb.Models.DTO
{
    public class DirectoryExportDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public List<DirectoryExportDTO> Children { get; set; } = new();
    }
}
