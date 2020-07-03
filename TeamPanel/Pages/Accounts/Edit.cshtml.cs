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

namespace TeamPanel.Pages
{
    public class AccountsEditModel : PageModel
    {
        private MySqlConnection MySQL;
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

        public void OnGet(string id, string redirect)
        {
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
                    Response.Redirect(Redirect);
                }
                else
                {
                    Response.Redirect("/Accounts");
                }
            }
        }

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Username)) { return Page(); }

            Account Account = MySQL.Get<Account>(Convert.ToInt32(Id));
            Account.USERNAME = Username;

            if (String.IsNullOrEmpty(Password))
            {
                Account.PASSWORD = MySQL.Get<Account>(Convert.ToInt32(Id)).PASSWORD;
            }
            else
            {
                string PasswordHash = Hash.CalculateBCryptPassword(Password);
                Account.PASSWORD = PasswordHash;
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