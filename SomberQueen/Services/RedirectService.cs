using System;
using System.IO;
using SomberQueen.Utilities;

namespace SomberQueen.Services
{
    public class RedirectService
    {
        private readonly DBHelper _dbHelper;

        public RedirectService(DBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        /*public IEnumerable<dynamic> GetLogs(int userId)
        {
            string query = "SELECT * FROM logs WHERE user_id = @userId ORDER BY created_at DESC";
            return _dbHelper.Query<dynamic>(query, new { userId = userId });
        }*/

        public void SaveUserRedirectLog(Guid userId, string action)
        {
            _dbHelper.Execute("INSERT INTO logs (user_id, action, created_at) VALUES (@user_id, @action, CURRENT_TIMESTAMP)",
            new
            {
                user_id = userId,
                action = action

            });
        }

      


    }
}
