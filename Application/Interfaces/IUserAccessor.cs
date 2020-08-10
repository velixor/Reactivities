using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        Task<AppUser> GetCurrentUser();
    }
}