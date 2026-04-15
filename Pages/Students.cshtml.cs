using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebObrasci1.Data;
using WebObrasci1.Models;

namespace WebObrasci1.Pages
{
    public class StudentsModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Student> Students { get; set; } = new List<Student>();

        [BindProperty]
        public Student NewStudent { get; set; }

        public StudentsModel(AppDbContext context)
        {
            _context = context; ;
        }
        public void OnGet()
        {
            Students = _context.Students.ToList();
        }

        public IActionResult OnPost()
        {
            _context.Students.Add(NewStudent);

            _context.SaveChanges();

            return RedirectToPage();
        }
    }
}
