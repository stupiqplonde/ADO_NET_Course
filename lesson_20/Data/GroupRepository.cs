using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class GroupRepository
    {
        private readonly string _conn_str;

        public GroupRepository(string connectionString)
        {
            _conn_str = connectionString;
        }

        public List<Group> GetAllGroups()
        {
            using var connection = new SqlConnection(_conn_str);
            return connection
                .Query<Group>("SELECT GroupId, GroupName FROM Groups ORDER BY GroupId")
                .ToList();
        }

        public int CreateGroup(string groupName)
        {
            using var connection = new SqlConnection(_conn_str);
            return connection
                .QuerySingle<int>("INSERT INTO Groups (GroupName) OUTPUT INSERTED.GroupId VALUES (@GroupName)",
                new { GroupName = groupName });
        }

        public Group GetGroupById(string id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection
                    .QueryFirstOrDefault<Group>("SELECT g.GroupId, g.GroupName, s.FirstName, s.LastName FROM Groups AS g, Students AS s WHERE g.GroupId = @id",
                    new { @id = id });
            }
        }
    }
}
