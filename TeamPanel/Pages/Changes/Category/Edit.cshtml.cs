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
    public class ChangesCategoryEditModel : PageModel
    {
        private MySqlConnection MySQL;
        public Account LoginUser;

        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Redirect { get; set; }

        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Content { get; set; }

        public ChangesCategoryEditModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(string id, string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            if (!string.IsNullOrEmpty(id))
            {
                ChangeCategory category = MySQL.Get<ChangeCategory>(Convert.ToInt32(id));
                Id = id;
                Title = category.TITLE;
                Redirect = redirect;
            }
            else
            {
                if (!String.IsNullOrEmpty(redirect))
                {
                    return Redirect(Redirect);
                }
                return Redirect("/Changes/Category");
            }

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Title)) { return Page(); }

            ChangeCategory category = MySQL.Get<ChangeCategory>(Convert.ToInt32(Id));
            category.TITLE = Title;

            MySQL.Update(category);

            if (!String.IsNullOrEmpty(Redirect))
            {
                return Redirect(Redirect);
            }
            return Redirect("/Changes/Category");
        }
    }
}