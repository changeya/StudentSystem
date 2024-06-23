using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Class_system_Backstage_pj.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Class_system_Backstage_pj.Areas.course_management.Controllers
{
    [Area("course_management")]
    public class T課程班級Controller : Controller
    {
        private readonly studentContext _context;

        public T課程班級Controller(studentContext context)
        {
            _context = context;
        }

        // GET: course_management/T課程班級
        public async Task<IActionResult> Index()
        {
            var studentContext = _context.T課程班級s.Include(t => t.班級導師);
            return View(await studentContext.ToListAsync());
        }

        public async Task<IActionResult> Indexjson()
        {
          

            var T課程班級 = await _context.T課程班級s
            .Where(t => t.狀態 == 1)
            .ToListAsync();
            return Json(T課程班級);
        }


        // GET: course_management/T課程班級/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T課程班級s == null)
            {
                return NotFound();
            }

            var t課程班級 = await _context.T課程班級s
                .Include(t => t.班級導師)
                .FirstOrDefaultAsync(m => m.班級id == id);
            if (t課程班級 == null)
            {
                return NotFound();
            }

            return View(t課程班級);
        }

   

        // GET: course_management/T課程班級/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T課程班級s == null)
            {
                return NotFound();
            }

            var t課程班級 = await _context.T課程班級s.FindAsync(id);
            if (t課程班級 == null)
            {
                return NotFound();
            }
            ViewData["班級導師id"] = new SelectList(_context.T會員老師s, "老師id", "信箱", t課程班級.班級導師id);
            return View(t課程班級);
        }

        // POST: course_management/T課程班級/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("班級id,班級名稱,班級導師id,入學日期,結訓日期,狀態")] T課程班級 t課程班級)
        {
            if (id != t課程班級.班級id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t課程班級);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T課程班級Exists(t課程班級.班級id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["班級導師id"] = new SelectList(_context.T會員老師s, "老師id", "信箱", t課程班級.班級導師id);
            return View(t課程班級);
        }

        // GET: course_management/T課程班級/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T課程班級s == null)
            {
                return NotFound();
            }

            var t課程班級 = await _context.T課程班級s
                .Include(t => t.班級導師)
                .FirstOrDefaultAsync(m => m.班級id == id);
            if (t課程班級 == null)
            {
                return NotFound();
            }

            return View(t課程班級);
        }

        // POST: course_management/T課程班級/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T課程班級s == null)
            {
                return Problem("Entity set 'studentContext.T課程班級s'  is null.");
            }
            var t課程班級 = await _context.T課程班級s.FindAsync(id);
            if (t課程班級 != null)
            {
                _context.T課程班級s.Remove(t課程班級);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T課程班級Exists(int id)
        {
          return (_context.T課程班級s?.Any(e => e.班級id == id)).GetValueOrDefault();
        }
    }
}
