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
    public class BetakeysDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public BetakeysDeleteModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(int id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;
            if (LoginUser.ROLE != "admin") { return Redirect("/Betakeys"); }

            if (!String.IsNullOrEmpty(Convert.ToString(id)))
            {
                BetaKey DeleteBetakey = MySQL.Get<BetaKey>(id);
                MySQL.Delete(DeleteBetakey);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Betakeys");
            }
            else
            {
                return Redirect(redirect);
            }
        }
    }
}