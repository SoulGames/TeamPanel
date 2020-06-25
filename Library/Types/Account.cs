using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("accounts")]
    public class Account
    {
        [ExplicitKey]
        int ID { get; set; }
        string USERNAME { get; set; }
        string PASSWORD { get; set; }
    }
}
