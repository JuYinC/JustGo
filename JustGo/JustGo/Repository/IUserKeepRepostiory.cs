using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IUserKeepRepostiory
    {
        public void Keep(UserKeepVM vm);
        public bool IsKeep(UserKeepVM vm);
    }
}
