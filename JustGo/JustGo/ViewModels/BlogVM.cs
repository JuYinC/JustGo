namespace JustGo.ViewModels
{
    public class BlogVM
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public string CoverImageName { get; set; }
        public int? Like { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<BlogDetailsVM> DetailsVMs { get; set; }
    }

    public partial class BlogDetailsVM
    {
        public DateTime StartTime { get; set; }
        public DateTime EndtTime { get; set; }
        public int PlaceId { get; set; }
        public string P_Name { get; set; }
        public string P_tel { get; set; }
        public string P_Add { get; set; }
        public string Describe { get; set; }
        public IList<string> Images { get; set; }
        public double? Score { get; set; }
    }
}
