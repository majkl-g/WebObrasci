using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;
using WebObrasci1.Services;

namespace WebObrasci1.Pages

{

    [Authorize(Roles = Role.Profesor)]
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<User> Users { get; set; } = new List<User>();

        [BindProperty]
        public User? NewUser { get; set; }

        public UsersModel(AppDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Users = _context.Users.ToList();
        }

        public IActionResult OnPost()
        {
            if (NewUser != null)
            {
                _context.Users.Add(NewUser);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }
    }
}
