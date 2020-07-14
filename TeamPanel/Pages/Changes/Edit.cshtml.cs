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
    public class ChangesEditModel : PageModel
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

        [BindProperty]
        public string Category { get; set; }
        [BindProperty]
        public ChangeCategory CurrentCategory { get; set; }

        public IEnumerable<ChangeCategory> Categories;

        public ChangesEditModel(MySqlConnection mySQL)
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
                Redirect = redirect;
                Id = id;

                Change Change = MySQL.Get<Change>(Convert.ToInt32(Id));
                Title = Change.TITLE;
                Content = Change.CHANGENEWS;
                CurrentCategory = MySQL.Get<ChangeCategory>(Change.CAT);
            }
            else
            {
                if (!String.IsNullOrEmpty(redirect))
                {
                    return Redirect(Redirect);
                }
                return Redirect("/Changes");
            }
            Categories = MySQL.GetAll<ChangeCategory>();

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (String.IsNullOrEmpty(Title)) { return Page(); }
            if (String.IsNullOrEmpty(Content)) { return Page(); }

            Change Change = MySQL.Get<Change>(Convert.ToInt32(Id));
            Change.TITLE = Title;
            Change.CHANGENEWS = Content;

            if (Category == "Undefined")
            {
                Change.CAT = 0;
            }
            else
            {
                if (Category == "Select this, to use the lastest.")
                {
                     Change.CAT = MySQL.Get<ChangeCategory>(Change.CAT).ID;
                }
                else
                {

                    foreach (ChangeCategory CurrentChangeCategory in MySQL.GetAll<ChangeCategory>())
                    {
                        if (CurrentChangeCategory.TITLE == Category)
                        {
                            Change.CAT = CurrentChangeCategory.ID;
                        }
                    }
                    if (String.IsNullOrEmpty(Convert.ToString(Change.CAT)))
                    {
                        Change.CAT = 0;
                    }
                }
            }

            MySQL.Update(Change);

            if (!String.IsNullOrEmpty(Redirect))
            {
                return Redirect(Redirect);
            }
            return Redirect("/Changes");
        }
    }
}