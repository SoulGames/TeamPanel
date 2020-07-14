using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Library.Managers;
using Library.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace TeamPanel.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public Account LoginUser;

        private readonly ILogger<ErrorModel> _logger;
        private MySqlConnection MySQL;

        public ErrorModel(ILogger<ErrorModel> logger, MySqlConnection mySQL)
        {
            _logger = logger;
            MySQL = mySQL;
        }

        public IActionResult OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Authorization
            AuthorizationResult authorizationResult;
            if (!Authorization.CheckAuthorization(HttpContext, MySQL, HttpContext.Response, out authorizationResult)) { return StatusCode(authorizationResult.StatusCode); }
            LoginUser = authorizationResult.Account;

            return Page();
        }
    }
}
