using herental.BL.Model;

namespace herental.BL.Interfaces
{
    public interface IProduct
    {
        string Name { get; }

        ProductType Type { get; }
    }
}