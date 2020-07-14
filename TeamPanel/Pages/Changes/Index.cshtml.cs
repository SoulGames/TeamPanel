using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Library.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Library.Managers;
using Library.Types;
using System.IO;

namespace TeamPanel.Pages
{
    public class ChangesIndexModel : PageModel
    {
        private MySqlConnection MySQL;

        public IEnumerable<Change> Changes;
        public Dictionary<int, string> Categories = new Dictionary<int, string>();

        public Account LoginUser;

        public ChangesIndexModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }

            Changes = MySQL.GetAll<Change>();

            Categories.Add(0, "Undefined");
            foreach(ChangeCategory CurrentChangeCategory in MySQL.GetAll<ChangeCategory>())
            { Categories.Add(CurrentChangeCategory.ID, CurrentChangeCategory.TITLE); }

            return Page();

        }
    }
}