using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dul.Articles;
using Dul.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ReactBoard1.Models
{
    public class EntryRepository : IEntryRepository, IDisposable
    {
        private readonly EntryDbContext _entryDbContext;
        private readonly ILogger _logger;

        public EntryRepository(EntryDbContext entryDbContext, ILoggerFactory loggerFactory)
        {
            _entryDbContext = entryDbContext;
            _logger = loggerFactory.CreateLogger(nameof(EntryRepository));
        }

        #region AddAsynce
        public async Task<Entry> AddAsync(Entry model)
        {
            try
            {
                _entryDbContext.Entries.Add(model);
                await _entryDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
               _logger?.LogError($"ERROR({nameof(AddAsync)} : {e.Message})");
            }
            return model;
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<Entry>> GetAllAsync()
        {
            return await _entryDbContext.Entries.OrderByDescending(m => m.Id).ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<Entry> GetByIdAsync(long id)
        {
            var model = await _entryDbContext.Entries.SingleOrDefaultAsync(m => m.Id == id);
            return model;
        }
        #endregion

        #region UpdateAsync
        public async Task<bool> UpdateAsync(Entry model)
        {
            try
            {
                _entryDbContext.Update(model);
                return (await _entryDbContext.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception e)
            {
                _logger?.LogError($"ERROR({nameof(UpdateAsync)} : {e.Message})");
            }
            return false;
        }
        #endregion

        #region DeleteAsync
        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var model = await _entryDbContext.Entries.FindAsync(id);
                _entryDbContext.Remove(model);
                return (await _entryDbContext.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception e)
            {
                _logger?.LogError($"ERROR({nameof(DeleteAsync)} : {e.Message})");
            }
            return false;
        }
        #endregion

        #region GetByAsync -ArticleSet
        public async Task<ArticleSet<Entry, long>> GetByAsync<TParentIdentifier>
            (FilterOptions<TParentIdentifier> options)
        {
            var items = _entryDbContext.Entries.AsQueryable();

            if (options.ChildMode)
            {
                if (options.ParentIdentifier is int parentId && parentId !=0)
                {
                    // items = items.where(m => m.ParentId == parentId);
                }
                else if (options.ParentIdentifier is string parentKey && !string.IsNullOrEmpty(parentKey)) ;
                {
                    // items = items.where(m => m.ParentKey == parentKey);
                }
            }

            if (!string.IsNullOrEmpty(options.SearchQuery))
            {
                if (options.SearchField == "Name")
                {
                    items = items.Where(m => m.Name.Contains(options.SearchQuery));
                }
                else if (options.SearchField == "Title")
                {
                    items = items.Where(m => m.Title.Contains(options.SearchQuery));
                }
                else if (options.SearchField == "Content")
                {
                    items = items.Where(m => m.Content.Contains(options.SearchQuery));
                }
                else
                {
                    items = items.Where(m =>
                        m.Name.Contains(options.SearchQuery) || m.Title.Contains(options.SearchQuery) || m.Content.Contains(options.SearchQuery));
                }
            }

            if (options.SortMode && options.SortFields != null)
            {
                foreach (var sf in options.SortFields)
                {
                    switch ($"{sf.Key}{sf.Value}")
                    {
                        case "NameAsc":
                            items = items.OrderBy(m => m.Name);
                            break;
                        case "NameDesc":
                            items = items.OrderByDescending(m => m.Name);
                            break;
                        case "TitleAsc":
                            items = items.OrderBy(m => m.Title);
                            break;
                        case "TitleDesc":
                            items = items.OrderByDescending(m => m.Title);
                            break;
                        default:
                            items = items.OrderByDescending(m => m.Id);
                            break;
                    }
                }
            }
            else
            {
                items = items.OrderByDescending(m => m.Id);
            }

            var totalCount = await items.CountAsync();

            items = items.Skip(options.PageIndex * options.PageSize).Take(options.PageSize);

            return new ArticleSet<Entry, long>(await items.ToListAsync(), totalCount);
        }
        #endregion

        #region GetAllAsync - PageResult
        public async Task<PagingResult<Entry>> GetAllPageAsync(int pageIndex, int pageSize)
        {
            var totalRecords = await _entryDbContext.Entries.CountAsync();
            var models = await _entryDbContext.Entries
                .OrderByDescending(m => m.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagingResult<Entry>(models, totalRecords);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_entryDbContext != null)
                {
                  _entryDbContext?.Dispose();
                }
            }
        }
        #endregion
    }

    public class EntryRepositoryAdoNet
    {
        // empty
    }

    public class EntryRepositoryDapper
    {
        // empty}
    }
}