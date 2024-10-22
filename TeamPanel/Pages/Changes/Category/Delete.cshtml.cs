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
    public class ChangesCategoryDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public ChangesCategoryDeleteModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(int id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            if (!String.IsNullOrEmpty(Convert.ToString(id)))
            {
                ChangeCategory DeleteCategory = MySQL.Get<ChangeCategory>(id);
                MySQL.Delete(DeleteCategory);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Changes/Category/");
            }
            else
            {
                return Redirect(redirect);
            }
        }
    }
}