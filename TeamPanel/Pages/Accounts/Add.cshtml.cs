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
    public class AccountsAddModel : PageModel
    {
        private MySqlConnection MySQL;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }


        public AccountsAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Password)) { return Page(); }
            if (String.IsNullOrEmpty(Username)) { return Page(); }

            string PasswordHash = Hash.CalculateBCryptPassword(Password);

            Account Account = new Account();
            Account.USERNAME = Username;
            Account.PASSWORD = PasswordHash;

            MySQL.Insert(Account);

            return Page();
        }
    }
}