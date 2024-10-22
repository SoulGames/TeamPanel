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
    public class AccountsDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public AccountsDeleteModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(int id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;
            if (LoginUser.ROLE != "admin") { return Redirect("/Accounts"); }

            if (!String.IsNullOrEmpty(Convert.ToString(id)))
            {
                Account DeleteAccount = MySQL.Get<Account>(id);
                MySQL.Delete(DeleteAccount);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Accounts");
            }
            else
            {
                return Redirect(redirect);
            }
        }
    }
}