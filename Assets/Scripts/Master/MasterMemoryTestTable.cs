using System.Collections;
using System.Collections.Generic;
using MasterMemory;
using MessagePack;
using UnityEngine;

[MemoryTable("TestTable"), MessagePackObject( true )]
public class MasterMemoryTestTable
{
    [PrimaryKey] public int Id { get; set; }
    public string TextVal { get; set; }
}
