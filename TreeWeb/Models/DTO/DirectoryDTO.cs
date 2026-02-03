namespace TreeWeb.Models.DTO
{
    public record DirectoryDTO
    {
        public string Name { get; init; } = "";
        public long? ParentId { get; init; }

        public DirectoryDTO(Directory dir)
        {
            Name = dir.Name;
            ParentId = dir.ParentId;
        }
        public DirectoryDTO()
        {

        }
    }  
}
