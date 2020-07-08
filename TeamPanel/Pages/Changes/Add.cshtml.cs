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
    public class ChangesAddModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        public IEnumerable<ChangeCategory> Categories;

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Content { get; set; }
        [BindProperty]
        public string Category { get; set; }

        public ChangesAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            Categories = MySQL.GetAll<ChangeCategory>();

            return Page();
        }


        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Title)) { return Page(); }
            if (String.IsNullOrEmpty(Content)) { return Page(); }

            Change change = new Change();
            change.TITLE = Title;
            change.CHANGENEWS = Content;
            change.CREATED_AT = DateTime.Now;

            if (Category == "Undefined")
            {
                change.CAT = 0;
            }
            else
            {
                foreach (ChangeCategory CurrentChangeCategory in MySQL.GetAll<ChangeCategory>())
                {
                    if (CurrentChangeCategory.TITLE == Category)
                    {
                        change.CAT = CurrentChangeCategory.ID;
                    }
                }
                if (String.IsNullOrEmpty(Convert.ToString(change.CAT)))
                {
                    change.CAT = 0;
                }
            }

            MySQL.Insert(change);

            return Redirect("/Changes");
        }
    }
}