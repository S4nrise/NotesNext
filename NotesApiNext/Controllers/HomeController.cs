using Microsoft.AspNetCore.Mvc;
using NotesApiNext.Database;

namespace NotesApiNext.Controllers
{
    public class HomeController(NotesNextDbContext notesNextDbContext) : Controller
    {
        public IActionResult Index()
        {
            var notes = notesNextDbContext.Notes.ToList();
            return View();
        }
    }
}
