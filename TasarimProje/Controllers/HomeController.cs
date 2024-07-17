using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TasarimProje.Models;
using DAL.Contexts;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;

namespace TasarimProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        VtContext vtContext2 = new VtContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index2()
        {
            ViewData["MaterialNo"] = new SelectList(vtContext2.Rol, "No", "Baslik");
            ViewBag.PM = vtContext2.Pm.ToList();
            ViewBag.Tur = vtContext2.Tur.ToList();
            ViewBag.Material = vtContext2.Material.ToList();


            var projeler = new List<Project>();

            projeler = new List<Project>();
            projeler = vtContext2.Project.ToList();
            var malzemes = (from pmT in vtContext2.Pm
                            join projects in vtContext2.Project on pmT.ProjectNo equals projects.No
                            join material in vtContext2.Material on pmT.MaterialNo equals material.No
                            select new
                            {
                                ProjectNo = projects.No,
                                MaterialBaslik = material.Baslik
                            }).ToList();

            ViewBag.malzemeler = malzemes;
            ViewBag.Users = vtContext2.Users.ToList();
            ViewBag.projeler = projeler;
            return View("Index");
        }

        [Route("Home/Index/{Id?}/{TurId?}")]
        public IActionResult Index(string id, string TurId)
        {
            ViewData["MaterialNo"] = new SelectList(vtContext2.Rol, "No", "Baslik");
            ViewBag.PM = vtContext2.Pm.ToList();
            ViewBag.Tur = vtContext2.Tur.ToList();
            ViewBag.Material = vtContext2.Material.ToList();
  

            var projeler = new List<Project>();
          
            if (!string.IsNullOrEmpty(id))
            {
                projeler = new List<Project>();

                string[] malzemeIds = id.Split(",");
                List<int> malzemeIdList = malzemeIds.Select(id => Convert.ToInt32(id)).ToList();

            
                int malzemeno = Convert.ToInt32(id);
                int turno = Convert.ToInt32(TurId);
                projeler = vtContext2.Project
                                 .Where(p => p.MaterialNo == malzemeno || p.TurNo== turno)

                                     //malzemeIdList.Contains((int)p.MaterialNo))
                                 .ToList();
                // malzemes sorgusunu filtreleyerek
                var malzemes = (from pmT in vtContext2.Pm
                                join projects in vtContext2.Project on pmT.ProjectNo equals projects.No
                                join material in vtContext2.Material on pmT.MaterialNo equals material.No
                                join Tur in vtContext2.Tur on projects.TurNo equals Tur.No
                                where material.No  == malzemeno || Tur.No == turno
                                //( malzemeIdList.Contains(material.No) || ) 
                                select new
                                {
                                    ProjectNo = projects.No,
                                    MaterialBaslik = material.Baslik
                                }).ToList();

                ViewBag.malzemeler = malzemes;
            }
            else
            {
                projeler = new List<Project>();
                projeler = vtContext2.Project.ToList();
                var malzemes = (from pmT in vtContext2.Pm
                                join projects in vtContext2.Project on pmT.ProjectNo equals projects.No
                                join material in vtContext2.Material on pmT.MaterialNo equals material.No
                                select new
                                {
                                    ProjectNo = projects.No,
                                    MaterialBaslik = material.Baslik
                                }).ToList();

                ViewBag.malzemeler = malzemes;
            }
            ViewBag.Users = vtContext2.Users.ToList();
            ViewBag.projeler = projeler;

            return View();
        }


 
        public async Task<IActionResult> UpdateUser(int? id)
        {
            if (id == null || vtContext2.Users == null)
            {
                return NotFound();
            }

            var users = await vtContext2.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "Ad");
            //ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "No", users.RolNo);
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUser( Users users)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    vtContext2.Update(users);
                    await vtContext2.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var user = vtContext2.Users.Find(users.No);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UsersDetails));
            }
            ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "Ad");
            //ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "No", users.RolNo);
            return View(users);
            

        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null || vtContext2.Users == null)
            {
                return NotFound();
            }

            var YorumList = vtContext2.ProjectYorum.Where(f => f.UsersNo == id.Value).ToList();

            foreach (var item in YorumList)
            {
                vtContext2.ProjectYorum.Remove(item);
                vtContext2.SaveChanges();

            }


            var FavoriList = vtContext2.Favori.Where(f => f.UsersNo == id.Value).ToList();

            foreach (var item in FavoriList)
            {
                vtContext2.Favori.Remove(item);
                vtContext2.SaveChanges();

            }


            var users =  vtContext2.Users.FirstOrDefault(f=> f.No == id.Value);
            vtContext2.Users.Remove(users);
            vtContext2.SaveChanges();

            ViewBag.Users = vtContext2.Users.ToList();
            return View("UsersDetails");
        }
        // GET: Projects/Delete/5
        public async Task<IActionResult> DeleteProject(int? id)
        {
            if (id == null || vtContext2.Project == null)
            {
                return NotFound();
            }
            var pmalzeme = vtContext2.Pm.Where(f => f.ProjectNo == id.Value).ToList();

            foreach (var item in pmalzeme)
            {
                vtContext2.Pm.Remove(item);
                vtContext2.SaveChanges();

            }
            var project = await vtContext2.Project
                .Include(p => p.MaterialNoNavigation)
                .Include(p => p.TurNoNavigation)
                .FirstOrDefaultAsync(m => m.No == id);
            if (project == null)
            {
                return NotFound();
            }

            if (project != null)
            {
                vtContext2.Project.Remove(project);
                vtContext2.SaveChanges();
            }
          
            return RedirectToAction(nameof(ProjeDetay));
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("DeleteProject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProjectConfirmed(int id)
        {
            if (vtContext2.Project == null)
            {
                return Problem("Entity set 'VtContext.Project'  is null.");
            }
            var project = await vtContext2.Project.FindAsync(id);
            if (project != null)
            {
                vtContext2.Project.Remove(project);
            }

            await vtContext2.SaveChangesAsync();
            return RedirectToAction(nameof(ProjeDetay));
        }




        // GET: Materials/Create
        public IActionResult MateryalCreate()
        {
            //ViewData["KategoriNo"] = new SelectList()
            ViewData["KategoriNo"] = new SelectList(vtContext2.Kategori, "No", "Ad");
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MateryalCreate([Bind("No,Baslik,KategoriNo")] Material material)
        {
            if (ModelState.IsValid)
            {
                vtContext2.Add(material);
                await vtContext2.SaveChangesAsync();
                return RedirectToAction(nameof(MateryalBilgisi));
            }
            ViewData["KategoriNo"] = new SelectList(vtContext2.Kategori, "No", "No", material.KategoriNo);
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> DeleteMaterial(int? id)
        {
            if (id == null || vtContext2.Material == null)
            {
                return NotFound();
            }

            var material = await vtContext2.Material
                .Include(m => m.KategoriNoNavigation)
                .FirstOrDefaultAsync(m => m.No == id);
            if (material != null)
            {
                vtContext2.Material.Remove(material);
                vtContext2.SaveChanges();
            }
            return RedirectToAction(nameof(MateryalBilgisi));

        //    return View("MateryalBilgisi");
        }

        //POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaterialConfirmed(int id)
        {
            if (vtContext2.Material == null)
            {
                return Problem("Entity set 'VtContext.Material'  is null.");
            }
            var material = await vtContext2.Material.FindAsync(id);
            if (material != null)
            {
                vtContext2.Material.Remove(material);
            }

            await vtContext2.SaveChangesAsync();
            return RedirectToAction(nameof(MateryalBilgisi));
        }

        private bool MaterialExists(int id)
        {
            return (vtContext2.Material?.Any(e => e.No == id)).GetValueOrDefault();
        }



        // GET: Projects/Create
        public IActionResult ProjeCreate()
        {
            ViewData["MaterialNo"] = new SelectList(vtContext2.Material, "No", "Baslik");
            ViewData["TurNo"] = new SelectList(vtContext2.Tur, "No", "Ad");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjeCreate([Bind("No,Ad,Explain,MaterialNo,TurNo")] Project project)
        {
            if (ModelState.IsValid)
            {
                vtContext2.Add(project);
                await vtContext2.SaveChangesAsync();
                return RedirectToAction(nameof(ProjeDetay));
            }
            ViewData["MaterialNo"] = new SelectList(vtContext2.Material, "No", "Baslik");
            ViewData["TurNo"] = new SelectList(vtContext2.Tur, "No", "Ad");
            return View(project);
        }




        // GET: Projects/Edit/5
        public async Task<IActionResult> UpdateProject(int? id)
        {
            if (id == null || vtContext2.Project == null)
            {
                return NotFound();
            }

            var project = await vtContext2.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            ViewData["MaterialNo"] = new SelectList(vtContext2.Material, "No", "Baslik");
            ViewData["TurNo"] = new SelectList(vtContext2.Tur, "No", "Ad");
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(int id, [Bind("No,Ad,Explain,MaterialNo,TurNo")] Project project)
        {
            if (id != project.No)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vtContext2.Update(project);
                    await vtContext2.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.No))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProjeDetay));
            }
             ViewData["MaterialNo"] = new SelectList(vtContext2.Material, "No", "Baslik");
            ViewData["TurNo"] = new SelectList(vtContext2.Tur, "No", "Ad");
            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return (vtContext2.Project?.Any(e => e.No == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> UpdateMaterial(int? id)
        {
            if (id == null || vtContext2.Material == null)
            {
                return NotFound();
            }

            var materyal = await vtContext2.Material.FindAsync(id);
            if (materyal == null)
            {
                return NotFound();
            }
            ViewData["KategoriNo"] = new SelectList(vtContext2.Kategori, "No", "Ad");
            //ViewData["RolNo"] = new SelectList(_context.Rol, "No", "No", users.RolNo);
            return View(materyal);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMaterial(Material materyal)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    vtContext2.Update(materyal);
                    await vtContext2.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var project = vtContext2.Project.Find(materyal.No);
                    if (materyal == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MateryalBilgisi));
            }
            //ViewData["RolNo"] = new SelectList(_context.Rol, "No", "No", users.RolNo);
            return View(materyal);


        }
        public ActionResult Login()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }
        public IActionResult Genel()
        {
            using (var db = new VtContext())
            {
                // Kullanıcı sayısını alıp ViewBag'e atama
                ViewBag.KategoriCount = db.Kategori.Count();
                ViewBag.TurCount = db.Tur.Count();
                ViewBag.UserCount = db.Users.Count();
                ViewBag.ProjectCount = db.Project.Count();
                ViewBag.MaterialCount = db.Material.Count();
            }

            ViewBag.Tur = vtContext2.Tur.ToList();
            ViewBag.Material = vtContext2.Material.ToList();
            ViewBag.projeler = vtContext2.Project.ToList();
            ViewBag.Users = vtContext2.Users.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            var KullaniciSorgu = vtContext2.Users.Include(x => x.RolNoNavigation).FirstOrDefault(i => i.Eposta == user.Eposta && i.Sifre == user.Sifre);
           
            if (KullaniciSorgu != null &&
            KullaniciSorgu.RolNo == 1  
           

            )

            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Ad", KullaniciSorgu.AdSoyad));
                //claims.Add(new Claim("soyad", User.Soyad));
                claims.Add(new Claim("Rol", KullaniciSorgu.RolNoNavigation.Ad));

                ViewBag.Tur = vtContext2.Tur.ToList();
                ViewBag.Material = vtContext2.Material.ToList();
                ViewBag.projeler = vtContext2.Project.ToList();
                ViewBag.Users = vtContext2.Users.ToList();

                var malzemes = (from a in vtContext2.Project
                                join b in vtContext2.Material on a.MaterialNo equals b.No
                                select b).ToList();


                ViewBag.malzemeler = malzemes;

                return RedirectToAction("Admin", "Home");
            
            }
            else
            {
                // Hatalı giriş işlemi
                TempData["ErrorMessage"] = "Hatalı giriş yaptınız.";
                return RedirectToAction("Login");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return View("/views/home/index.cshtml");

        }

        public IActionResult Admin()

        {
            using (var db = new VtContext())
            {
                // Kullanıcı sayısını alıp ViewBag'e atama
                ViewBag.KategoriCount = db.Kategori.Count();
                ViewBag.TurCount = db.Tur.Count();
                ViewBag.UserCount = db.Users.Count();
                ViewBag.ProjectCount = db.Project.Count();
                ViewBag.MaterialCount = db.Material.Count();
            }


         

            return View();
        }

        // GET: Users/Create
        public IActionResult UserCreate()
        {
            ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "Ad");
            return View();
        }

        // POST: Users/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCreate([Bind("No,AdSoyad,KullaniciAd,Sifre,Eposta,RolNo")] Users users)
        {
            if (ModelState.IsValid)
            {
                vtContext2.Add(users);
                await vtContext2.SaveChangesAsync();
                return RedirectToAction(nameof(UsersDetails));
            }
            ViewData["RolNo"] = new SelectList(vtContext2.Rol, "No", "No", users.RolNo);
            return View(users);
        }



        public IActionResult Hakkında()
        {
            return View();

        }
    

        public IActionResult ProjeDetay()
        {
            ViewBag.ProjectCount = vtContext2.Project.Count();
            ViewBag.Tur = vtContext2.Tur.ToList();
            ViewBag.Material = vtContext2.Material.ToList();
            ViewBag.Projeler = vtContext2.Project.ToList().OrderByDescending(o => o.No).ToList();
            return View();
        }
        public IActionResult UsersDetails()
        {
            ViewBag.UserCount = vtContext2.Users.Count();

            ViewBag.Users = vtContext2.Users.ToList().OrderByDescending(o => o.No).ToList();
            return View();
        }

        public IActionResult MateryalBilgisi()
        {


            ViewBag.MateryalCount = vtContext2.Material.Count();
            ViewBag.Material = vtContext2.Material.OrderByDescending(o => o.No).
                Include(x=>x.KategoriNoNavigation).ToList() ;
            return View();
        }

      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


