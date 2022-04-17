using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using School_MIS.Models;

namespace School_MIS.Controllers
{
    public class EnrollmentsController : Controller
    {
        private db_SchoolMISEntities db = new db_SchoolMISEntities();

        // GET: Enrollments
        public async Task<ActionResult> Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student).Include(e => e.Lecture);
            return View(await enrollments.ToListAsync());
        }

        public PartialViewResult _enrollmentPartial(int? courseid)
        {
            var enrollments = db.Enrollments.Where(x => x.CourseID == courseid)
                .Include(e => e.Course)
                .Include(e => e.Student);
            return PartialView(enrollments.ToList());
        }
        // GET: Enrollments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName");
            ViewBag.LecturesId = new SelectList(db.Lectures, "Id", "FirstName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturesId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturesId = new SelectList(db.Lectures, "Id", "FirstName", enrollment.LecturesId);
            return View(enrollment);
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent([Bind(Include = "CourseID,StudentID")] Enrollment enrollment)
        {
            try
            {
                var isEnrolled = db.Enrollments.Any(x => x.StudentID == enrollment.StudentID && x.CourseID == enrollment.CourseID);

                if (ModelState.IsValid && !isEnrolled)
                {
                    db.Enrollments.Add(enrollment);
                    await db.SaveChangesAsync();
                    return Json(new { IsSuccess = true, Message = "Student Added Successfully !" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { IsSuccess = false, Message = "Student is already enrolled !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "System Failure: Please Contact Your Administrator !" }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: Enrollments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturesId = new SelectList(db.Lectures, "Id", "FirstName", enrollment.LecturesId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturesId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturesId = new SelectList(db.Lectures, "Id", "FirstName", enrollment.LecturesId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            db.Enrollments.Remove(enrollment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetStudents(string term)
        {
            var students = db.Students.Select(x => new {
                FullName = x.FirstName + " " + x.LastName,
                Id = x.StudentID
            }).Where(x => x.FullName.Contains(term));

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
