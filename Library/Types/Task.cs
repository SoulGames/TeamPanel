using Dapper.Contrib.Extensions;
using System;

namespace Library.Types
{
    [Table("tasks")]
    public class Task
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string TITLE { get; set; }
        public string PROJECT { get; set; }
        public int? ASSIGNED { get; set; }
        public int? CREATOR { get; set; }
        public DateTime CREATED_AT { get; set; }
        public bool DONE { get; set; }
    }
}
