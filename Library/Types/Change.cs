using Dapper.Contrib.Extensions;
using System;

namespace Library.Types
{
    [Table("changelog")]
    public class Change
    {
        [ExplicitKey]
        int ID { get; set; }
        string TITLE { get; set; }
        string CHANGENEWS { get; set; }
        DateTime CREATED_AT { get; set; }
        string CAT { get; set; }
    }
}
