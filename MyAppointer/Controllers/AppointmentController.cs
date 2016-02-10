using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MyAppointer.Models;

namespace MyAppointer.Controllers
{
    public class AppointmentController : ApiController
    {
        private MyAppointerEntities db = new MyAppointerEntities();

        // GET api/Appointment
        public IEnumerable<Appointments> GetAppointments()
        {
            var appointments = db.Appointments.Include(a => a.JobOwners).Include(a => a.Users);
            return appointments.AsEnumerable();
        }

        // GET api/Appointment/5
        public Appointments GetAppointments(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return appointments;
        }

        // PUT api/Appointment/5
        public HttpResponseMessage PutAppointments(int id, Appointments appointments)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != appointments.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(appointments).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Appointment
        public HttpResponseMessage PostAppointments(Appointments appointments)
        {
            int overlap_flag = 0;
            HttpResponseMessage response = new HttpResponseMessage();

            if (ModelState.IsValid)
            {
                foreach(Appointments appointment in db.Appointments)
                {
                    if (appointment.BookDate == appointments.BookDate) {
                        if ((appointment.StartTime <= appointments.StartTime) && (appointment.EndTime >= appointments.StartTime)) {
                            overlap_flag = 1;
                            break;
                        }else if ((appointment.EndTime >= appointments.EndTime) && (appointment.StartTime <= appointments.EndTime))
                        {
                            overlap_flag = 1;
                            break;
                        }
                    } 
                }

                if (overlap_flag == 1)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, "overlap");
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = appointments.Id }));
                }
                else
                {
                    db.Appointments.Add(appointments);
                    db.SaveChanges();

                    response = Request.CreateResponse(HttpStatusCode.Created, appointments);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = appointments.Id }));
                }

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Appointment/5
        public HttpResponseMessage DeleteAppointments(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Appointments.Remove(appointments);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, appointments);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}