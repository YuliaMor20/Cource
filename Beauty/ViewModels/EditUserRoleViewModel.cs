using Microsoft.AspNetCore.Identity;

namespace Beauty.ViewModels
{
    public class EditUserRoleViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IList<string> UserRoles { get; set; }
        public IList<IdentityRole> AllRoles { get; set; }
        public IList<string> SelectedRoles { get; set; }
    }
}
