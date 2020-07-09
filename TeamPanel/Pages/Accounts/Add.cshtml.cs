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
    public class AccountsAddModel : PageModel
    {
        private MySqlConnection MySQL;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public Account LoginUser;


        public AccountsAddModel(MySqlConnection mySQL)
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

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Password)) { return Page(); }
            if (String.IsNullOrEmpty(Username)) { return Page(); }

            string PasswordHash = Hash.CalculateBCryptPassword(Password);

            Account Account = new Account();
            Account.USERNAME = Username;
            Account.PASSWORD = PasswordHash;
            Account.USING_BY = 2;

            MySQL.Insert(Account);

            return Page();
        }
    }
}