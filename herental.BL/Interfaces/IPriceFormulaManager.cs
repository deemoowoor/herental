namespace herental.BL.Interfaces
{
    public interface IPriceFormulaManager
    {
        decimal CalculatePrice(string typeName, int period);
    }
}