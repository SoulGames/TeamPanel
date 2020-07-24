using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Library.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Library.Managers;
using Library.Types;
using System.IO;
using System.Runtime.InteropServices;
using Dapper;

namespace TeamPanel.Pages
{
    public class TasksIndexModel : PageModel
    {
        private MySqlConnection MySQL;

        public List<Library.Types.Task> Tasks = new List<Library.Types.Task>();
        public Library.Types.Task Task;

        public Dictionary<int?, Account> Accounts  = new Dictionary<int?, Account>();

        public Account LoginUser;

        public TasksIndexModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(int? id, string mode, string done)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }

            if (String.IsNullOrEmpty(Convert.ToString(id)))
            {
                IEnumerable<Library.Types.Task> AllTasks;
                if (!(done == "true"))
                {
                    AllTasks = MySQL.Query<Library.Types.Task>("SELECT * FROM tasks WHERE DONE=FALSE;");
                }
                else
                {
                    AllTasks = MySQL.GetAll<Library.Types.Task>();
                }
                switch (mode)
                {
                    case "created":
                        foreach(Library.Types.Task CurrentTask in AllTasks)
                        {
                            if(CurrentTask.CREATOR.Equals(authorizationResult.Account.ID)) { Tasks.Add(CurrentTask); }
                        }
                        break;
                    case "assigned":
                        foreach (Library.Types.Task CurrentTask in AllTasks)
                        {
                            if (CurrentTask.ASSIGNED.Equals(authorizationResult.Account.ID)) { Tasks.Add(CurrentTask); }
                        }
                        break;
                    case "me":
                        foreach (Library.Types.Task CurrentTask in AllTasks)
                        {
                            if (CurrentTask.ASSIGNED.Equals(authorizationResult.Account.ID)) { Tasks.Add(CurrentTask); break; }
                            if (CurrentTask.CREATOR.Equals(authorizationResult.Account.ID)) { Tasks.Add(CurrentTask); }
                        }
                        break;
                    default:
                        Tasks = AllTasks.ToList();
                        break;
                }
            }
            else
            {
                Task = MySQL.Get<Library.Types.Task>(id);
            }

            foreach(Account CurrentAccount in MySQL.GetAll<Account>())
            {
                Accounts.Add(CurrentAccount.ID, CurrentAccount);
            }

            return Page();

        }
    }
}