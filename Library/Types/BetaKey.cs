using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("betakeys")]
    public class BetaKey
    {
        [ExplicitKey]
        int ID { get; set; }
        string BETAPW { get; set; }
    }
}
