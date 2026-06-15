using LaserAPI.Dal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace LaserAPITests.Mock
{
    /// <summary>
    /// Backs the component tests with a real EF Core <see cref="DataContext"/> running on an
    /// in-memory SQLite database. Unlike the Moq-based mocks, this exercises the actual
    /// Include/ThenInclude relationship loading in the DALs. The database lives for as long as
    /// the connection stays open, so the instance must be disposed when the test finishes.
    /// </summary>
    internal sealed class MockedDataContext : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<DataContext> _options;

        public MockedDataContext()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection)
                .Options;

            using DataContext context = NewContext();
            context.Database.EnsureCreated();
        }

        /// <summary>
        /// Returns a fresh context over the same in-memory database, mirroring the scoped
        /// DataContext that each DAL receives per request in production.
        /// </summary>
        public DataContext NewContext() => new(_options);

        public void Dispose() => _connection.Dispose();
    }
}
