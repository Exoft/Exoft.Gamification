namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IPasswordHasher
    {
        string GetHash(string input);
        bool Equals(string input, string valueToCompare);
    }
}
