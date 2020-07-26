using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Library.Types
{
    [Table("entries")]
    public class Entry
    {
        [ExplicitKey]
        public string NAME { get; set; }
        public string VALUE { get; set; }
    }
}
