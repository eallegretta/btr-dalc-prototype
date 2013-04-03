namespace BlogTalkRadio.Common.Data
{
    public interface IQuery<T> where T : class, new()
    {
        DataSource DataSource { get; }
    }
}
