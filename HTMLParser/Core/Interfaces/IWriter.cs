using System.Threading.Tasks;

namespace HTMLParser.Core.Interfaces
{
    interface IWriter<T> where T : class
    {
        string BaseDirectory { get; set; }
        string FileName { get; set; }
        string FullPath { get; }
        void Write(T obj);
    }
}
