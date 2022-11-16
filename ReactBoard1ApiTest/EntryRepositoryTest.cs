using System;
using System.Linq;
using System.Threading.Tasks;
using Dul.Articles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactBoard1.Models;

namespace ReactBoard1ApiTest
{
    [TestClass] 
    public class EntryRepositoryTest
    {
        // Microsoft.EntityFramework.InMemory
        [TestMethod]
        public async Task EntryRepositoryAllMethodTest()
        {
            #region Creating Object and ILoggerFactory

            var options = new DbContextOptionsBuilder<EntryDbContext>()
                .UseInMemoryDatabase($"reactboard1{Guid.NewGuid()}").Options;

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();

            #endregion

            #region AddAsync()

            using (var context = new EntryDbContext(options))
            {
                // Checking if the DB is created
                context.Database.EnsureCreated();

                var repository = new EntryRepository(context, factory);
                var model = new Entry { Name = "Admin", Title = "Title", Content = "Tell me something" };

                await repository.AddAsync(model);
            }

            using (var context = new EntryDbContext(options))
            {
                Assert.AreEqual(1, await context.Entries.CountAsync());
                var model = await context.Entries.Where(m => m.Id == 1).SingleOrDefaultAsync();
                Assert.AreEqual("Admin", model.Name);
            }

            #endregion

            #region GetAllAsync()

            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                var model = new Entry { Name = "Ms Jennifer Lee", Title = "Dogs", Content = "GoldenDoodles" };

                await repository.AddAsync(model);
                await repository.AddAsync(new Entry
                    { Name = "YeraLee", Title = "HorribleCamp", Content = "Nobody comes out of the camp" });
            }

            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                var models = await repository.GetAllAsync();
                Assert.AreEqual(3, models.Count());
            }

            #endregion

            #region GetByIDAsync()

            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                var model = await repository.GetByIdAsync(3);
                Assert.IsTrue(model.Name.Contains("Yera"));
                Assert.AreEqual("YeraLee", model.Name);
            }

            #endregion

            #region UPdateAsync()

            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                var model = await repository.GetByIdAsync(2);
                model.Name = "QueenKong";
                await repository.UpdateAsync(model);

                var updateModel = await repository.GetByIdAsync(2);

                Assert.IsTrue(updateModel.Name.Contains("Queen"));
                Assert.AreEqual("QueenKong", updateModel.Name);
                Assert.AreEqual("QueenKong",
                    (await context.Entries.Where(m => m.Id == 2).SingleOrDefaultAsync())?.Name);
            }

            #endregion

            #region DeleteAsync()
            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                await repository.DeleteAsync(2);

                Assert.AreEqual(2, (await context.Entries.CountAsync()));
                Assert.IsNull(await repository.GetByIdAsync(2));
            }
            #endregion

            #region GetByAsync
            using (var context = new EntryDbContext(options))
            {
                var repository = new EntryRepository(context, factory);
                var filter = new FilterOptions<long>(){PageIndex = 0, PageSize = 1}; // bring one item from the first page
                var entriesSet = await repository.GetByAsync<long>(filter);
                var firstName = entriesSet.Items.FirstOrDefault()?.Name;
                var totalCount = entriesSet.TotalCount;
                
                Assert.AreEqual("YeraLee", firstName);
                Assert.AreEqual(2, totalCount);
            }
            #endregion

            #region GetByAllPageAsync
            using (var context = new EntryDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1;

                var repository = new EntryRepository(context, factory);
                var entriesSet = await repository.GetAllPageAsync(pageIndex, pageSize);

                var firstName = entriesSet.Records.FirstOrDefault()?.Name;
                var recordCount = entriesSet.TotalRecords;
            }
            #endregion
        }
    }
}