using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArabaKiralama.Data;
using ArabaKiralama.Models;

namespace ArabaKiralama.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CarsController(ApplicationDbContext context) { _context = context; }

        // LİSTELEME
        public async Task<IActionResult> Index() => View(await _context.Cars.ToListAsync());

        // EKLEME SAYFASI
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // SİLME
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}