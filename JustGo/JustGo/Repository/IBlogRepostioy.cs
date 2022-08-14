using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IBlogRepostioy
    {
        public BlogVM createScheduleToBlog(int scheduleId);

        public bool editBlog(BlogVM vm);

        public bool createBlog(BlogVM vm);

        public bool deleteBlog(BlogVM vm);

        public ICollection<BlogVM> selectUserBlog(string userId);

        public BlogVM selectBlog(int blogId);

        public ICollection<BlogVM> getBlogFilter(SelectPlaceVM vm);

        public ICollection<BlogVM> getBlogRank();

        public ICollection<BlogVM> getKeepBlog(UserKeepVM vm);
    }
}
