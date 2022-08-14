using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IUserKeepRepostiory
    {
        public bool Keep(UserKeepVM vm);
        public bool IsKeep(UserKeepVM vm);
    }
}
