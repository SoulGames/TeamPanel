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
using MySql.Data.MySqlClient;
using Library.Managers;
using Library.Types;

namespace TeamPanel.Pages
{
    public class ChangesAddModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        public ChangesAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            return Page();
        }
    }
}