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
    public class AccountsDeleteModel : PageModel
    {
        private MySqlConnection MySQL;
        public AccountsDeleteModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet(int id, string redirect)
        {
            if (!String.IsNullOrEmpty(Convert.ToString(id)))
            {
                Account DeleteAccount = MySQL.Get<Account>(id);
                MySQL.Delete(DeleteAccount);
            }
            if (String.IsNullOrEmpty(redirect))
            {
                Response.Redirect("/Accounts");
            }
            else
            {
                Response.Redirect(redirect);
            }
        }
    }
}