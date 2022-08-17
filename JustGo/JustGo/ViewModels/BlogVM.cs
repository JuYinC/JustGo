namespace JustGo.ViewModels
{
    public class BlogVM
    {
        public int BlogId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? Title { get; set; }
        public string? Describe { get; set; }        
        public blogImage? CoverImage { get; set; }
        public int? Like { get; set; }
        public IList<int>? TagList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IList<IList<BlogDetailsVM>>? Details { get; set; }
    }

    public partial class BlogDetailsVM
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int PlaceId { get; set; }
        public string? P_Name { get; set; }
        public string? P_tel { get; set; }
        public string? P_Add { get; set; }
        public string? Describe { get; set; }
        public IList<blogImage>? Images { get; set; }
        public double? Score { get; set; }
    }

    public class blogImage
    {
        public string? name { get; set; }
        public string? base64 { get; set; }
    }
}
