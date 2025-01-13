using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Data.Sqlite;
using PersonalManager.Models;
using System.Diagnostics;

namespace PersonalManager.Config
{
    public class AppDbContext
    {
        private readonly string _dbPath;

        public AppDbContext(string dbName)
        {
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Combine(folderPath, dbName);
            /* if (File.Exists(_dbPath))
             {
                 File.Delete(_dbPath);
                 Debug.WriteLine("Baza danych została usunięta.");
             } 
             CreateTableAsync<Event>().Wait();
             CreateTableAsync<MyTask>().Wait();
             CreateTableAsync<Note>().Wait();
             CreateTableAsync<Reminder>().Wait();
            CreateTableAsync<CheckItem>().Wait();
            CreateTableAsync<Checklist>().Wait();
            */



        }

        private IDbConnection CreateConnection()
{
    return new SqliteConnection($"Data Source={_dbPath}");
}



        public async Task CreateTableAsync<T>()
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id"); // Pomijamy właściwość "Id"

            var columns = properties.Select(p => $"{p.Name} {GetColumnType(p.PropertyType)}");
            var sql = $@"
        CREATE TABLE IF NOT EXISTS [{tableName}] (
            Id INTEGER PRIMARY KEY AUTOINCREMENT, 
             { string.Join(",", columns)}
        ); ";
 
    await connection.ExecuteAsync(sql);
        }


        public async Task<int> InsertAsync<T>(T entity)
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id"); // Ignorujemy Id
            var columns = string.Join(",", properties.Select(p => $"[{p.Name}]"));
            var values = string.Join(",", properties.Select(p => $"@{p.Name}"));
            var sql = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values});";

            return await connection.ExecuteAsync(sql, entity);
        }
       
        public async Task<int> UpdateAsync<T>(T entity)
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties().Where(p => p.Name != "Id");
            var setClause = string.Join(",", properties.Select(p => $"[{p.Name}] = @{p.Name}"));
            var sql = $"UPDATE [{tableName}] SET {setClause} WHERE Id = @Id;";

            return await connection.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync<T>(int id)
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var sql = $"DELETE FROM [{tableName}] WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<List<T>> GetAllAsync<T>()
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM [{tableName}];";
            var result = await connection.QueryAsync<T>(sql);
            return result.ToList();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM [{tableName}] WHERE Id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<T> GetWithRelationsAsync<T, TRel1, TRel2>(int id, string sql)
        {
            using var connection = CreateConnection();
            var data = await connection.QueryAsync<T, TRel1, TRel2, T>(
                sql,
                (entity, rel1, rel2) =>
                {
                    // Opcjonalne przypisanie relacji do modelu głównego
                    var relProp1 = typeof(T).GetProperty(typeof(TRel1).Name);
                    var relProp2 = typeof(T).GetProperty(typeof(TRel2).Name);
                    relProp1?.SetValue(entity, rel1);
                    relProp2?.SetValue(entity, rel2);
                    return entity;
                },
                new { Id = id },
                splitOn: "Id");

            return data.FirstOrDefault();
        }
        public async Task<List<string>> GetAllTableNamesAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT name FROM sqlite_master WHERE type='table';";
            var result = await connection.QueryAsync<string>(sql);
            return result.ToList();
        }

        private string GetColumnType(Type type)
        {
            return type switch
            {
                var t when t == typeof(int) || t == typeof(long) => "INTEGER",
                var t when t == typeof(string) => "TEXT",
                var t when t == typeof(DateTime) => "TEXT",
                var t when t == typeof(bool) => "INTEGER",
                var t when t == typeof(double) || t == typeof(float) => "REAL",
                _ => "TEXT",
            };
        }
        public async Task<T> FindAsync<T>(int id)
        {
            using var connection = CreateConnection();
            var tableName = typeof(T).Name;
            var sql = $"SELECT * FROM [{tableName}] WHERE Id = @Id;";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<int> AddNoteAsync(Note note)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO Note (Content, EventId) VALUES (@Content, @EventId);";
            return await connection.ExecuteAsync(sql, note);
        }

        public async Task<List<Note>> GetNotesByEventIdAsync(int eventId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Note WHERE EventId = @EventId;";
            var notes = await connection.QueryAsync<Note>(sql, new { EventId = eventId });
            return notes.ToList();
        }
        public async Task<int> UpdateNoteAsync(Note note)
        {
            using var connection = CreateConnection();
            var sql = "UPDATE Note SET Content = @Content WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, note);
        }
        public async Task<int> AddChecklistAsync(Checklist checklist)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO Checklist (Title, EventId, MyTaskId) VALUES (@Title, @EventId, @MyTaskId);";
            return await connection.ExecuteAsync(sql, checklist);
        }
        public async Task<int> UpdateChecklistAsync(Checklist checklist)
        {
            using var connection = CreateConnection();
            var sql = "UPDATE Checklist SET Title = @Title, EventId = @EventId, MyTaskId = @MyTaskId WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, checklist);
        }
        public async Task<List<Checklist>> GetAllChecklistsAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Checklist;";
            var checklists = await connection.QueryAsync<Checklist>(sql);
            return checklists.ToList();
        }
        public async Task<List<Checklist>> GetChecklistsByEventIdAsync(int eventId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Checklist WHERE EventId = @EventId;";
            var checklists = await connection.QueryAsync<Checklist>(sql, new { EventId = eventId });
            return checklists.ToList();
        }
        public async Task<int> DeleteChecklistAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Checklist WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<int> AddCheckItemAsync(CheckItem checkItem)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO CheckItem (Content, IsCompleted, ChecklistId) VALUES (@Content, @IsCompleted, @ChecklistId);";
            return await connection.ExecuteAsync(sql, checkItem);
        }
        public async Task<List<CheckItem>> GetCheckItemsByChecklistIdAsync(int checklistId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM CheckItem WHERE ChecklistId = @ChecklistId;";
            var items = await connection.QueryAsync<CheckItem>(sql, new { ChecklistId = checklistId });
            return items.ToList();
        }
        public async Task<int> UpdateCheckItemAsync(CheckItem checkItem)
        {
            using var connection = CreateConnection();
            var sql = "UPDATE CheckItem SET Content = @Content, IsCompleted = @IsCompleted WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, checkItem);
        }

        public async Task<int> DeleteCheckItemAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM CheckItem WHERE Id = @Id;";
            return await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Checklist> GetChecklistWithItemsAsync(int checklistId)
        {
            using var connection = CreateConnection();
            var sqlChecklist = "SELECT * FROM Checklist WHERE Id = @Id;";
            var checklist = await connection.QueryFirstOrDefaultAsync<Checklist>(sqlChecklist, new { Id = checklistId });

            if (checklist != null)
            {
                var sqlItems = "SELECT * FROM CheckItem WHERE ChecklistId = @ChecklistId;";
                var items = await connection.QueryAsync<CheckItem>(sqlItems, new { ChecklistId = checklistId });
                foreach (var item in items)
                {
                    item.ChecklistId = checklistId;
                }
            }

            return checklist;
        }





    }
}
