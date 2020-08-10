using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IAppUserAccessor
    {
        Task<AppUser> GetCurrentUser();
    }
}