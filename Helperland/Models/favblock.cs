namespace Helperland.Models
{
    public class favblock
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TargetUserId { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsBlocked { get; set; }
        public int Id { get; set; }
    }
}
