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
    public class BetakeysAddModel : PageModel
    {
        private MySqlConnection MySQL;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public Account LoginUser;


        public BetakeysAddModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public IActionResult OnGet(string redirect)
        {
            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            Random random = new Random();

            BetaKey NewBetaKey = new BetaKey();
            NewBetaKey.BETAPW = String.Format("{0}-{1}-{2}-{3}", GenerateRandomLetters(4), GenerateRandomLetters(4), GenerateRandomLetters(4), GenerateRandomLetters(4));

            MySQL.Insert(NewBetaKey);

            if (String.IsNullOrEmpty(redirect))
            {
                return Redirect("/Betakeys");
            }
            return Redirect(redirect);
        }



        public static string GenerateRandomLetters(int amount) // Thanks to https://stackoverflow.com/a/49922533/13114227
        {
            Random r = new Random();
            string[] letters = { "a", "b", "c", "d","e", "f", "g", "h", "i", "j", "k", "m", "l", "n", "o", "p", "q", "r", "s", "t","u", "v", "w", "x" , "y", "z"};
            string Name = "";
            int b = 0;
            while (b < amount)
            {
                Name += letters[r.Next(letters.Length)];
                b++;
            }

            return Name.ToUpper();
        }
    }
}