using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using Library.Managers;
using Library.Types;
using Dapper;
using Dapper.Contrib.Extensions;

namespace TeamPanel.Pages
{
    public class IndexModel : PageModel
    {
        private MySqlConnection MySQL;

        public Account LoginUser;

        public List<Library.Types.Task> Tasks = new List<Library.Types.Task>();
        public Library.Types.Task Task;

        public IEnumerable<Account> AllUsers;
        public Dictionary<int?, Account> Accounts = new Dictionary<int?, Account>();

        public IndexModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            IEnumerable<Library.Types.Task> AllTasks;
            AllTasks = MySQL.Query<Library.Types.Task>("SELECT * FROM tasks WHERE DONE=FALSE;");
            foreach (Library.Types.Task CurrentTask in AllTasks)
            {
                if (CurrentTask.ASSIGNED.Equals(authorizationResult.Account.ID)) { Tasks.Add(CurrentTask); }
            }

            foreach (Account CurrentAccount in MySQL.GetAll<Account>())
            {
                Accounts.Add(CurrentAccount.ID, CurrentAccount);
            }
            AllUsers = MySQL.GetAll<Account>();

            return Page();
        }
    }
}
