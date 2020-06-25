using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("betausers")]
    public class BetaUser
    {
        [ExplicitKey]
        int ID { get; set; }
        string USERNAME { get; set; }
        int isB { get; set; }

    }
}
