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
    public class BetausersAddModel : PageModel
    {
        private MySqlConnection MySQL;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string redirect { get; set; }

        public Account LoginUser;
        public BetausersAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            return Page();
        }
        public IActionResult OnPostAsync()
        {
            BetaUser NewBetaUser = new BetaUser();
            NewBetaUser.USERNAME = Username;
            MySQL.Insert(NewBetaUser);

            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Betausers");
            }
            return Redirect(redirect);
        }
    }
}