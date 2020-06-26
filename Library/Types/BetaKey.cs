using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("betakeys")]
    public class BetaKey
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string BETAPW { get; set; }
    }
}
