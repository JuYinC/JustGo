using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IUserKeepRepository
    {
        public bool Keep(UserKeepVM vm);
        public bool IsKeep(UserKeepVM vm);
    }
}
