namespace Buoi2.Models.ViewModels
{
    public class UserClaimViewModel
    {
        //Ctor
        public UserClaimViewModel()
        {
            Claims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }
    }
}
