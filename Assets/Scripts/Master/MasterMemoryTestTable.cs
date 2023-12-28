using MasterMemory;
using MessagePack;

namespace Master
{
    [MemoryTable("TestTable"), MessagePackObject( true )]
    public class MasterMemoryTestTable : ITestTable
    {
        [PrimaryKey] public int Id { get; set; }
        public string TextVal { get; set; }
    }
}
