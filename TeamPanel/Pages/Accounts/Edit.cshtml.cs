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
    public class AccountsEditModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;
        public AccountsEditModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Redirect { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnGet(string id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;
            if (LoginUser.ROLE != "admin") { return Redirect("/Accounts"); }

            if (!string.IsNullOrEmpty(id))
            {
                Account Account = MySQL.Get<Account>(Convert.ToInt32(id));
                Id = id;
                Username = Account.USERNAME;
                Redirect = redirect;
            }
            else {
                if (!String.IsNullOrEmpty(redirect))
                {
                    return Redirect(Redirect);
                }
                else
                {
                    return Redirect("/Accounts");
                }
            }

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Username)) { return Page(); }

            Account Account = MySQL.Get<Account>(Convert.ToInt32(Id));
            Account.USERNAME = Username;

            if (String.IsNullOrEmpty(Password))
            {
                Account.PASSWORD = MySQL.Get<Account>(Convert.ToInt32(Id)).PASSWORD;
                Account.USING_BY = MySQL.Get<Account>(Convert.ToInt32(Id)).USING_BY;
            }
            else
            {
                string PasswordHash = Hash.CalculateBCryptPassword(Password);
                Account.PASSWORD = PasswordHash;
                Account.USING_BY = 2;
            }


            MySQL.Update(Account);

            if (!String.IsNullOrEmpty(Redirect))
            {
                return Redirect(Redirect);
            }
            return Redirect("/Accounts");
        }
    }
}