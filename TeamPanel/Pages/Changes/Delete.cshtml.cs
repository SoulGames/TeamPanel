using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Library.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Library.Managers;
using Library.Types;

namespace TeamPanel.Pages
{
    public class ChangesDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public ChangesDeleteModel(MySqlConnection mySQL)
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
                Change DeleteChange = MySQL.Get<Change>(id);
                MySQL.Delete(DeleteChange);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Changes");
            }
            else
            {
                return Redirect(redirect);
            }
        }
    }
}