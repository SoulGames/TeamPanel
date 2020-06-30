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

namespace TeamPanel.Pages
{
    public class AccountsModel : PageModel
    {
        private MySqlConnection MySQL;

        public IEnumerable<Account> Accounts;

        public AccountsModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet()
        {
            Accounts = MySQL.GetAll<Account>();
        }
    }
}