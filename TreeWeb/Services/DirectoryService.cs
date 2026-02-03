using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using TreeWeb.Abstract;
using TreeWeb.AppContext;
using TreeWeb.Models.DTO;

namespace TreeWeb.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly TreeWebDbContext _dbContext;

        private readonly ILogger<DirectoryService> _logger;

        /// <summary>
        /// SQL запрос: идем от нового родителя вверх до самого корня 
        /// </summary>
        private readonly string sqlQueryForCycleReference = @"
            WITH RECURSIVE Hierarchy(Id, ParentId) AS (
                SELECT Id, ParentId FROM {2} WHERE Id = {0}
                UNION ALL
                SELECT d.Id, d.ParentId 
                FROM {2} d
                JOIN Hierarchy h ON d.Id = h.ParentId
            )
            SELECT EXISTS (SELECT 1 FROM Hierarchy WHERE Id = {1}) as Value";

        public DirectoryService(TreeWebDbContext dbContext, ILogger<DirectoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<(DirectoryDTO result, string error)> AddDirectoryAsync(DirectoryDTO directoryDTO)
        {
            var entity = new Models.Directory
            {
                Name = directoryDTO.Name,
                ParentId = directoryDTO.ParentId
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            await _dbContext.Directories.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("Создана директория: {Name}", entity.Name);
            return (result: new DirectoryDTO(entity), null);
            
        }

        public async Task<bool> DeleteDirectoryAsync(long id)
        {
            var dir = await _dbContext.Directories.FirstOrDefaultAsync(d => d.Id == id);
            if(dir !=null)
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                _dbContext.Directories.Remove(dir);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<DirectoryExportDTO>> GetDirectoriesAsync()
        {
            return await GetDirectoryTreeAsync();
        }

        public async Task<DirectoryDTO> GetDirectoryAsync(long id)
        {
            var dir = await _dbContext.Directories
                  .Include(d => d.Children)
                  .FirstOrDefaultAsync(d => d.Id == id);
            if(dir == null)
            {
                return null;
            }
            return new DirectoryDTO(dir);
        }

        public async Task<(DirectoryDTO result, string error)> UpdateDirectoryAsync(long id, DirectoryDTO dto)
        {
            var dir = await _dbContext.Directories.FindAsync(id);
            if (dir == null) return (null,null);

            string directoryTableName = _dbContext.Model.FindEntityType(typeof(TreeWeb.Models.Directory))?.GetTableName();
            if(directoryTableName == null)
            {
                throw new SystemException("Нет таблицы для хранения каталогов");
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            if (await IsCircularReferenceSql(dir.Id, dto.ParentId, directoryTableName))
            {
                await transaction.DisposeAsync();
                _logger.LogWarning("Попытка создания циклической ссылки для ID: {Id}", id);
                return (null, "Обновление вызывает цикл!");
            }

            dir.Name = dto.Name;
            dir.ParentId = dto.ParentId;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return (new DirectoryDTO(dir),null);
        }

        private async Task<bool> IsCircularReferenceSql(long? Id, long? newParentId, string tableName)
        {
            if (newParentId == null) return false;
            if (Id == newParentId) return true;
                     
            // Выполняем через EF Core
            var cycleQuery = _dbContext.Database
                .SqlQueryRaw<int>(string.Format(sqlQueryForCycleReference, newParentId, Id, tableName));

            var isCycle = await cycleQuery.FirstOrDefaultAsync();
            return isCycle == 1;
        }

        public async Task<List<DirectoryExportDTO>> GetDirectoryTreeAsync()
        {
            _logger.LogInformation("Запрос на экспорт всей структуры.");
            // Получаем все записи из БД одним запросом
            var allItems = await _dbContext.Directories
                .Select(d => new DirectoryExportDTO { Id = d.Id, Name = d.Name, ParentId = d.ParentId })
                .ToListAsync();

            // Создаем словарь для быстрого доступа
            var dict = allItems.ToDictionary(d => d.Id);
            var rootNodes = new List<DirectoryExportDTO>();

            foreach (var item in allItems)
            {
                if (item.ParentId.HasValue && dict.TryGetValue(item.ParentId.Value, out var parent))
                {
                    parent.Children.Add(item);
                }
                else
                {
                    rootNodes.Add(item);
                }
            }
            return rootNodes;
        }
    }
}
