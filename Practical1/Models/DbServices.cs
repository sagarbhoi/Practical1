using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Practical1.Models
{
    public class DbServices
    {
        private readonly Practical1Entities db;
        public DbServices(Practical1Entities practical1Entities)
        {
            db = practical1Entities;
        }
        public User UserAuthenticate(User user)
        {
            var r = db.Users.Where(x => x.Email.Equals(user.Email) && x.Password.Equals(user.Password) && x.Role.Equals(user.Role)).SingleOrDefault();
            return r;
        }
        public IEnumerable<Event> GetAllEvents()
        {
            return db.Events.ToList();
        }
        public Event GetEventbyId(int ?id)
        {
            return db.Events.FirstOrDefault(x=>x.Event_Id==id);
        }
        public bool EditEvent(Event e)
        {
            e.Description = e.Description.Trim();
            e.Title = e.Title.Trim();
            try
            {
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool SaveEvent(Event e)
        {
            try
            {
                db.Events.Add(e);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool RemoveEventById(int id)
        {
            try
            {
                db.Events.Remove(GetEventbyId(id));
                var e = db.Event_User.Where(x => x.Event_Id == id);
                foreach (var item in e)
                {
                    db.Event_User.Remove(item);

                }
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public  AssignEvent AssignEvent()
        {
            AssignEvent assignEvent = new AssignEvent();
            assignEvent.users = db.Users.Where(x => x.Role.Equals("User")).ToList();
            assignEvent.events = db.Events.ToList();
            assignEvent.event_user = db.Event_User.ToList(); ;
            return assignEvent;

        }
        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }
        public User GetUserbyId(int? id)
        {
            return db.Users.FirstOrDefault(x => x.U_id == id);
        }
        public bool EditUser(User u)
        {
            try
            {
                
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool SaveUser(User e)
        {
            try
            {
                db.Users.Add(e);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool RemoveUserById(int id)
        {
            try
            {
                db.Users.Remove(GetUserbyId(id));
                db.Event_User.RemoveRange(db.Event_User.Where(x => x.U_id == id).ToList());
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Event_User GetEvent_UserbyId(int? id)
        {
            var r= db.Event_User.Where(x => x.id == id).FirstOrDefault();
            return r;
           
        }
        public bool AddEvent_User(IEnumerable<Event_User> event_Users)
        {
            try
            {
                db.Event_User.AddRange(event_Users);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool DeleteEvent_UserById(int id)
        {
            try
            {
                var r = GetEvent_UserbyId(id);
                db.Event_User.Remove(r);
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}