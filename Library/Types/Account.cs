using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("accounts")]
    public class Account
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
    }
}
