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
    public class IndexModel : PageModel
    {
        private MySqlConnection MySQL;

        public IndexModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet()
        {

        }
    }
}
