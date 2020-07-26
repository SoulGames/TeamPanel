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
    public class TasksDoneModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        public IEnumerable<ChangeCategory> Categories;

        public TasksDoneModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(string id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            if (!string.IsNullOrEmpty(id))
            {
                Library.Types.Task task = MySQL.Get<Library.Types.Task>(Convert.ToInt32(id));

                if (task.DONE)
                {
                    task.DONE = false;
                }
                else
                {
                    task.DONE = true;
                }

                MySQL.Update(task);
            }

            if (!String.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }
            return Redirect("/Tasks");
        }
    }
}