using Dapper.Contrib.Extensions;
using System;

namespace Library.Types
{
    [Table("changelog")]
    public class Change
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string TITLE { get; set; }
        public string CHANGENEWS { get; set; }
        public DateTime CREATED_AT { get; set; }
        public string CAT { get; set; }
    }
}
