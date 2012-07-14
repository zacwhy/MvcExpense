
namespace Zac.DesignPattern.Models
{
    public abstract class PersistentObject<T>
    {
        public T Id { get; set; }
    }
}
