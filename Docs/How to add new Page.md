# How to add new Page

#### NewPage.cshtml.cs

```csharp
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
    public class NewPageModel : PageModel
    {
        private MySqlConnection MySQL;

        public NewPageModel(MySqlConnection mySQL)
        {
            MySQL = mySQL;
        }

        public void OnGet()
        {

        }
    }
}
```

#### NewPage.cshtml

```csharp
@page
@model NewPageModel
@{
    ViewData["Title"] = "New Page";
}

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Hey, this is the Content of the new Page <code>:D</code></p>
</div>
```
