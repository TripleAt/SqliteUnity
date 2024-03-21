using Cysharp.Threading.Tasks;

public interface ITableRepository
{
	UniTask<ITestTable> GetDataAsync(int index);
}