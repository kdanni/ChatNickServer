using MySql.Data.Entity;
using System.Data.Common;
using System.Data.Entity;

namespace NickServer
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class MemoDbContext : DbContext
	{
		public DbSet<Memo> Memos{ get; set; }

		public DbSet<User> Users{ get; set; }

		public DbSet<Peer> Peers{ get; set; }


		public MemoDbContext (DbConnection contextString, bool contextOwnsConnection) 
			: base (contextString, contextOwnsConnection)
		{

		}
	}
}

