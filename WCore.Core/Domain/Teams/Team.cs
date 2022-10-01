namespace WCore.Core.Domain.Teams
{
    public partial class Team : BaseEntity
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }

        public int TeamCategoryId { get; set; }
        public TeamCategory TeamCategory { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public bool ShowOn { get; set; }
        public bool ShowOnHome { get; set; }
    }
}
