using TreeWeb.Models.DTO;

namespace TreeWeb.Abstract
{
    public interface IDirectoryService
    {
        Task<(DirectoryDTO result,string error)> AddDirectoryAsync(DirectoryDTO directoryDTO);
        Task<DirectoryDTO> GetDirectoryAsync(long id);
        Task<IEnumerable<DirectoryExportDTO>> GetDirectoriesAsync();
        Task<(DirectoryDTO result, string error)> UpdateDirectoryAsync(long id, DirectoryDTO directoryDTO);
        Task<bool> DeleteDirectoryAsync(long id);
    }
}
