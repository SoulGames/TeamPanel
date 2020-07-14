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

namespace TeamPanel.Pages
{
    public class BetausersDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public BetausersDeleteModel(MySqlConnection mySQL)
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
                BetaUser DeleteBetauser = MySQL.Get<BetaUser>(id);
                MySQL.Delete(DeleteBetauser);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Betausers");
            }
            else
            {
                return Redirect(redirect);
            }
        }
    }
}