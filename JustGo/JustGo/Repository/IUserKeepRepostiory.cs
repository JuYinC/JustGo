using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IUserKeepRepostiory
    {
        public void KeepBlog(UserKeepVM vm);        
        public IList<int> GetUserKeep(UserKeepVM vm);        
    }
}
