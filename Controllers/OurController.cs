using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OurController : Controller
    {
        // GET: Our
        public ActionResult index()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
                return View();
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult addTeam()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
                return View();
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult checking()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
                return View();
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult leaderBoard()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
                return View();
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult newsFeed()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var news = db.news.ToList();
                    news.Sort((x, y) => DateTime.Compare(y.dateUpload, x.dateUpload));

                    return View(news);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult schedule()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var fixture = db.fixtures.ToList();

                    var fixtures = fixture.OrderBy(x => x.time);
                    var newFixture = fixtures.Where(f => f.time > DateTime.Now).ToList();


                    foreach (var fix in newFixture)
                    {
                        fix.HomeTeam = db.Teams.Find(fix.HomeTeamId);
                        fix.AwayTeam = db.Teams.Find(fix.AwayTeamId);
                    }

                    return View(newFixture);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ViewPlayers(int id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var Players = db.players.Where(p => p.TeamID == id).ToList();
                    Players[0].Team = db.Teams.Find(id);

                    return View(Players);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult teams(int id = 1)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var Teams = db.Teams.ToList().Take(id*10).Skip((id-1)*10);

                    ViewBag.pageIndex = id;
                    return View(Teams);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult usersDashboard(int id = 1)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using(MyDatabase db = new MyDatabase())
                {
                    var Users = db.Users.ToList().Take(id*10).Skip((id-1)*10);
                    ViewBag.pageIndex = id;
                    return View(Users);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult AddNewTeam(string name, HttpPostedFileBase flag, string sports_type, string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8, string p9, string p10, string p11, string p12, string p13, string p14, string p15, string p16)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                if (flag != null)
                {
                    string _FileName = Path.GetFileName(flag.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/images/"), _FileName);
                    flag.SaveAs(_path);

                    string[] players = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16 };

                    using (MyDatabase db = new MyDatabase())
                    {
                        var Team = db.Teams.Where(team => team.TeamName == name).ToList();
                        if (Team.Count > 0) return RedirectToAction("index");
                        else
                        {
                            int count = 0;
                            for (int i = 0; i < 16; i++)
                            {
                                if (players[i] == null) break;
                                count++;
                            }

                            db.Teams.Add(new Models.Team(name, "/Content/images/" + sports_type.ToLower() + "-logo.jpg", "/Content/images/" + _FileName, count));

                            db.SaveChanges();

                            int id = db.Teams.Where(t => t.TeamName == name).ToList()[0].Id;

                            for (int i = 0; i < count; i++)
                            {
                                db.players.Add(new Player(players[i], id));
                            }

                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DeleteTeam(int id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    db.Teams.Remove(db.Teams.Find(id));

                    db.SaveChanges();
                }
                return RedirectToAction("teams");
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult AddNews(string post_description, HttpPostedFileBase file)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                if (file != null)
                {
                    try
                    {
                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Content/images/"), _FileName);
                        file.SaveAs(_path);

                        using (MyDatabase db = new MyDatabase())
                        {
                            db.news.Add(new News(post_description, "/Content/images/" + _FileName, DateTime.Now));

                            db.SaveChanges();
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    if (post_description.Length > 0)
                    {
                        using (MyDatabase db = new MyDatabase())
                        {
                            db.news.Add(new News(post_description, "", DateTime.Now));

                            db.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("newsFeed");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult AddFixture()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var teams = db.Teams.ToList();

                    return View(teams);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult AddNewFixture(DateTime date, string team1, string team2)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                if (team1 == team2 || date < DateTime.Now) return RedirectToAction("index");
                if (team1 != "" && team2 != "")
                {
                    using (MyDatabase db = new MyDatabase())
                    {
                        var ALlFixtures = db.fixtures.ToList();
                        foreach (var fix in ALlFixtures)
                        {
                            if (fix.time < DateTime.Now)
                            {
                                db.fixtures.Remove(fix);
                            }
                        }

                        db.fixtures.Add(new Fixture(Int16.Parse(team1), Int16.Parse(team2), date));

                        db.SaveChanges();
                    }
                }

                return RedirectToAction("index");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteUser(int id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == true)
            {
                using(MyDatabase db = new MyDatabase())
                {
                    db.Users.Remove(db.Users.Find(id));
                    var posts = db.Posts.ToList();
                    foreach(var post in posts)
                    {
                        if(post.UserID == id) db.Posts.Remove(post);
                    }
                    var friends = db.Friends.ToList();
                    foreach(var friend in friends)
                    {
                        if (friend.SenderFriendID1 == id || friend.RecieverFriendID2 == id) db.Friends.Remove(friend);
                    }
                    var frs = db.FriendRequests.ToList();
                    foreach(var fr in frs)
                    {
                        if(fr.SenderID ==id || fr.RecieverID == id)
                        {
                            db.FriendRequests.Remove(fr);
                        }
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("usersDashboard");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}