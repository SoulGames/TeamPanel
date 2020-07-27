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

namespace TeamPanel.Pages
{
    public class AccountsIndexModel : PageModel
    {
        private MySqlConnection MySQL;

        public IEnumerable<Account> Accounts;

        public Account LoginUser;

        public AccountsIndexModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;
            if (LoginUser.ROLE != "admin") { return Redirect("/"); }


            Accounts = MySQL.GetAll<Account>();

            return Page();

        }
    }
}