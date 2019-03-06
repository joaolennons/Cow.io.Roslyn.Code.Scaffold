namespace Coding
{
    public interface ISourceCode
    {
        string FileName { get; }
        string Folder { get; }
        string Code { get; }
    }
}
