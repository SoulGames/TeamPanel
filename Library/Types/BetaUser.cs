using Dapper.Contrib.Extensions;

namespace Library.Types
{
    [Table("betausers")]
    public class BetaUser
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public int isB { get; set; }

    }
}
