using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace TeamPanel.Pages
{
    public class PrivacyModel : PageModel
    {
        private MySqlConnection MySQL;

        public PrivacyModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet()
        {
        }
    }
}
