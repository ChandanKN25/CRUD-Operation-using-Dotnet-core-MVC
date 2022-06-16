using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCCoreWebAppln.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCoreWebAppln.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDBContext _db;

        [BindProperty]
        public Book Book { get; set; }

        public BooksController(ApplicationDBContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

       

        //public async Task<IActionResult> Create(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _db.Books.AddAsync(book);
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));

        //    }
        //    else
        //    {
        //        return View();
        //    }
           
        //}

        //public async Task<IActionResult> Edit(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var BookFromDb = await _db.Books.FindAsync(book.Id);
        //        BookFromDb.Name = book.Name;
        //        BookFromDb.Author = book.Author;
        //        await _db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));

        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var BookFromDb = await _db.Books.FindAsync(id);
        //    if (BookFromDb == null)
        //    {
        //        return NotFound();

        //    }
        //    _db.Books.Remove(BookFromDb);
        //    await _db.SaveChangesAsync();
        //    return RedirectToPage("Index");

        //}

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id== null)
            {
                return View(Book);
            }
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id ==0)
                {
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Book);

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OnPostdelete(int id)
        {
            var BookFromDb = await _db.Books.FindAsync(id);
            if(BookFromDb == null )
            {
                return NotFound();
            }
               _db.Books.Remove(BookFromDb);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
