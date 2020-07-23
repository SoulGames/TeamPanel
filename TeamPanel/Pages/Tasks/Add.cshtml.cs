using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Library.Managers;
using Library.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Library.Managers;
using Library.Types;

namespace TeamPanel.Pages
{
    public class TasksAddModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        [BindProperty]
        public int LoginUserId {get; set;}


        public IEnumerable<Account> AllUsers;

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Project { get; set; }
        [BindProperty]
        public string Assign { get; set; }

        public TasksAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;
            LoginUserId = LoginUser.ID;

            AllUsers = MySQL.GetAll<Account>();

            return Page();
        }


        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Title)) { return Page(); }
            if (String.IsNullOrEmpty(Project)) { return Page(); }

            Library.Types.Task task = new Library.Types.Task();
            task.TITLE = Title;
            task.PROJECT = Project;
            task.CREATOR = LoginUserId;
            task.CREATED_AT = DateTime.Now;

            if (!(Assign == "Not set"))
            {
                foreach (Account CurrentAccount in MySQL.GetAll<Account>())
                {
                    if (CurrentAccount.USERNAME == Assign)
                    {
                        task.ASSIGNED = CurrentAccount.ID;
                    }
                }
            }

            MySQL.Insert(task);

            return Redirect("/Tasks");
        }
    }
}