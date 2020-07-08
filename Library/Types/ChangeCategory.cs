using Dapper.Contrib.Extensions;
using System;

namespace Library.Types
{
    [Table("category")]
    public class ChangeCategory
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string TITLE { get; set; }
    }
}
