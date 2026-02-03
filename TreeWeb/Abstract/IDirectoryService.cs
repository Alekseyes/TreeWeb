using TreeWeb.Models.DTO;

namespace TreeWeb.Abstract
{
    public interface IDirectoryService
    {
        /// <summary>
        /// Добавляет каталог 
        /// </summary>
        /// <param name="directoryDTO"></param>
        /// <returns>Выдаёт ли результат, либо ошибку</returns>
        Task<(DirectoryDTO? result,string? error)> AddDirectoryAsync(DirectoryDTO directoryDTO);
        /// <summary>
        /// Отдает инфу по одному каталогу 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DirectoryDTO> GetDirectoryAsync(long id);
        /// <summary>
        /// Отдает каталоги с иерархией
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DirectoryExportDTO>> GetDirectoriesAsync();

        /// <summary>
        /// Редактирование каталога
        /// </summary>
        /// <param name="id">id каталога</param>
        /// <param name="directoryDTO"> сущность с изменениями</param>
        /// <returns> Выдаёт ли результат, либо ошибку</returns>
        Task<(DirectoryDTO? result, string? error)> UpdateDirectoryAsync(long id, DirectoryDTO directoryDTO);
        
        /// <summary>
        /// Удаление каталога
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Удалось/не удалось удалить</returns>
        Task<bool> DeleteDirectoryAsync(long id);
    }
}
