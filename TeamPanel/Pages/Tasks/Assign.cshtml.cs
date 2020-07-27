using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TasksAssignModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        public IEnumerable<ChangeCategory> Categories;

        public TasksAssignModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(int id, string redirect, int user)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            if (!string.IsNullOrEmpty(Convert.ToString(id)))
            {
                Task task = MySQL.Get<Library.Types.Task>(Convert.ToInt32(id));

                if (!string.IsNullOrEmpty(Convert.ToString(user)))
                {
                    task.ASSIGNED = user;
                }
                else
                {
                    task.ASSIGNED = null;
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