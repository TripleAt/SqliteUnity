using SQLite;

namespace Tables
{
	[System.Serializable]
	public class TestTable : ITestTable
	{
		[AutoIncrement, PrimaryKey, Indexed]
		public int Id { get; set; }
		[MaxLength(64)]
		public string TextVal{ get; set; }
	}
}
