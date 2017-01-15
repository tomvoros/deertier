using Dapper;
using Dapper.FastCrud;
using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using System;
using System.Linq;
using System.Transactions;

namespace DeerTier.Web.Data
{
    public class LeaderboardRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public LeaderboardRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public bool DeleteRecord(Record record, string IPAddress, string moderator = "admin")
        {
            try
            {
                using (var trans = new TransactionScope())
                using (var conn = _connectionProvider.GetConnection())
                {
                    conn.Delete(record);
                    
                    conn.Execute(@"
                        INSERT INTO tblRecordDeletionLog (Moderator, CategoryId, DeletionDate, Player, RealTimeString, GameTimeString, RealTimeSeconds, GameTimeSeconds, Comment, VideoURL, CeresTime, DateSubmitted, SubmittedByUserId, IPAddress)
                        VALUES (@Moderator, @CategoryId, GetDate(), @Player, @RealTimeString, @GameTimeString, @RealTimeSeconds, @GameTimeSeconds, @Comment, @VideoURL, @CeresTime, @DateSubmitted, @SubmittedByUserId, @IPAddress);", 
                        new
                        {
                            Moderator = moderator,
                            CategoryId = record.CategoryId,
                            Player = record.Player,
                            RealTimeString = record.RealTimeString,
                            GameTimeString = record.GameTimeString,
                            RealTimeSeconds = record.RealTimeSeconds,
                            GameTimeSeconds = record.GameTimeSeconds,
                            Comment = record.Comment,
                            VideoURL = record.VideoURL,
                            CeresTime = record.CeresTime,
                            DateSubmitted = record.DateSubmitted,
                            SubmittedByUserId = record.SubmittedByUserId,
                            IPAddress = IPAddress
                        });

                    trans.Complete();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ScoreDeletionRecordModel[] GetScoreDeletionLog()
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Query<ScoreDeletionRecordModel>("SELECT * FROM tblRecordDeletionLog ORDER BY ID DESC").ToArray();
            }
        }
        
        public Category[] GetCategories()
        {
            var sql = @"
SELECT * FROM refSections ORDER BY ID
SELECT * FROM tblCategories WHERE Enabled = 1 ORDER BY ID
";

            using (var conn = _connectionProvider.GetConnection())
            {
                var reader = conn.QueryMultiple(sql);

                var sections = reader.Read<Section>().ToArray();
                var categories = reader.Read<Category>().ToArray();

                foreach (var category in categories)
                {
                    if (category.SectionId.HasValue)
                    {
                        category.Section = sections.FirstOrDefault(s => s.Id == category.SectionId.Value);
                    }

                    if (category.ParentId.HasValue)
                    {
                        category.Parent = categories.FirstOrDefault(c => c.Id == category.ParentId.Value);
                    }
                }

                foreach (var category in categories)
                {
                    category.Subcategories = categories.Where(c => c.Parent == category).OrderBy(c => c.DisplayOrder).ToArray();
                    category.DefaultSubcategory = category.Subcategories.FirstOrDefault();
                }

                foreach (var section in sections)
                {
                    section.Categories = categories.Where(c => c.Section == section).OrderBy(c => c.DisplayOrder).ToArray();
                }

                return categories;
            }
        }

        public Record GetRecord(int id)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Get(new Record { Id = id });
            }
        }

        public Record[] GetRecords(int categoryId)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Query<Record>("SELECT * FROM tblRecords WHERE CategoryId = @CategoryId", new { CategoryId = categoryId }).ToArray();
            }
        }

        public void AddRecord(Record record)
        {
            using (var trans = new TransactionScope())
            using (var conn = _connectionProvider.GetConnection())
            {
                // Check if record already exists
                var exists = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM tblRecords WHERE Player = @Player AND CategoryId = @CategoryId", new { Player = record.Player, CategoryId = record.CategoryId }) > 0;

                if (exists)
                {
                    // Delete existing record
                    conn.Execute("DELETE FROM tblRecords WHERE Player = @Player AND CategoryId = @CategoryId", new { Player = record.Player, CategoryId = record.CategoryId });
                }

                // Insert new record
                conn.Insert(record);

                trans.Complete();
            }
        }

        public Record[] GetAllRecords()
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Find<Record>().ToArray();
            }
        }
    }
}
