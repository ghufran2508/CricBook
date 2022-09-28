using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UserViewController : Controller
    {
        // GET: User
        public ActionResult index()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                var ID = Int16.Parse(Session["Id"].ToString());
                if (ID == null) return RedirectToAction("index", "Home");
                using (MyDatabase db = new MyDatabase())
                {
                    var User = db.Users.Find(ID);
                    var FollowersCount = db.Friends.ToList().Where(friend => friend.SenderFriendID1 == (int)ID).Count();
                    var FollowingCount = db.Friends.ToList().Where(friend => friend.RecieverFriendID2 == (int)ID).Count();
                    var Fixtures = db.fixtures.ToList().Select(f => f).Take(5);
                    var AllFriends = db.Friends.ToList().Where(friend => friend.SenderFriendID1 == User.ID || friend.RecieverFriendID2 == User.ID).ToList();
                    var AllFriendsR = db.FriendRequests.ToList().Where(friend => friend.SenderID == User.ID || friend.RecieverID == User.ID).ToList();
                    var AllUsers = db.Users.ToList().Where(user => user.ID != User.ID);
                    var UsersExceptMe = AllUsers.ToList();

                    List<User> suggestions = new List<User>();

                    for (int i = 0; i < UsersExceptMe.Count; i++)
                    {
                        bool found = false;
                        for (int j = 0; j < AllFriends.Count; j++)
                        {
                            if (AllFriends[j].SenderFriendID1 == UsersExceptMe[i].ID || AllFriends[j].RecieverFriendID2 == UsersExceptMe[i].ID)
                            {
                                found = true;
                            }
                        }
                        if (found == false) suggestions.Add(UsersExceptMe[i]);
                        if (suggestions.Count == 10) break;
                    }
                    for (int i = 0; i < suggestions.Count; i++)
                    {
                        bool exist = false;
                        for (int j = 0; j < AllFriendsR.Count; j++)
                        {
                            if (suggestions[i].ID == AllFriendsR[j].SenderID || suggestions[i].ID == AllFriendsR[j].RecieverID)
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist)
                        {
                            suggestions.RemoveAt(i);
                            i--;
                        }
                    }


                    UserIndexView myView = new UserIndexView();
                    myView.myProfile = User;
                    myView.followersCount = FollowersCount;
                    myView.followingCount = FollowingCount;
                    myView.fixtures = Fixtures;
                    myView.suggestions = suggestions;

                    var MyPosts = db.Posts.ToList().Where(post => post.UserID == User.ID).ToList();
                    List<Post> posts = new List<Post>();
                    foreach (var post in MyPosts)
                    {
                        post.User = User;
                        posts.Add(post);
                    }
                    foreach (var friends in AllFriends)
                    {
                        if (friends.SenderFriendID1 == User.ID)
                        {
                            var friendPost = db.Posts.ToList().Where(p => p.UserID == friends.RecieverFriendID2).ToList();

                            var fUser = db.Users.Find(friends.RecieverFriendID2);

                            foreach (var fpOst in friendPost)
                            {
                                fpOst.User = fUser;
                                posts.Add(fpOst);
                            }
                        }
                        else
                        {
                            var friendPost = db.Posts.ToList().Where(p => p.UserID == friends.SenderFriendID1).ToList();

                            var fUser = db.Users.Find(friends.SenderFriendID1);

                            foreach (var fpOst in friendPost)
                            {
                                fpOst.User = fUser;
                                posts.Add(fpOst);
                            }
                        }
                    }

                    var news = db.news.ToList();
                    foreach(var n in news)
                    {
                        Post nP = new Post(-1, n.Description, n.Media, n.dateUpload);
                        User admin = new User("ADMIN", "", "", "/Content/images/Sports-Logo-Designs-1280x720.jpg");
                        nP.User = admin;
                        posts.Add(nP);
                    }


                    posts.Sort((x, y) => DateTime.Compare(y.dateUpload, x.dateUpload));

                    myView.posts = posts;


                    var fixture = db.fixtures.ToList();

                    var fixtures = fixture.OrderBy(x => x.time);
                    var newFixture = fixtures.Where(f => f.time > DateTime.Now).ToList();


                    foreach (var fix in newFixture)
                    {
                        fix.HomeTeam = db.Teams.Find(fix.HomeTeamId);
                        fix.AwayTeam = db.Teams.Find(fix.AwayTeamId);
                    }
                    myView.fixtures = newFixture;

                    return View(myView);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult changeProfilePic(HttpPostedFileBase new_profile_pic)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                try
                {
                    if (new_profile_pic != null)
                    {
                        string _FileName = Path.GetFileName(new_profile_pic.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Media"), _FileName);

                        if (System.IO.File.Exists(_path)) {
                            //Console.WriteLine("EXIST");
                            string NewFileName = _FileName.Split('.')[0];
                            string extension = _FileName.Split('.')[1];

                            int count = 0;
                            string FolderFileName = NewFileName + count+"." + extension;
                            string _path2 = Path.Combine(Server.MapPath("~/Media"), FolderFileName);

                            while (System.IO.File.Exists(_path2))
                            {
                                count++;
                                FolderFileName = NewFileName + count+"." + extension;
                                _path2 = Path.Combine(Server.MapPath("~/Media"), FolderFileName);
                            }

                            _FileName = FolderFileName;
                            _path = Path.Combine(Server.MapPath("~/Media"), _FileName);
                        }

                        new_profile_pic.SaveAs(_path);

                        using (MyDatabase db = new MyDatabase())
                        {
                            var Me = db.Users.Find((int)Session["Id"]);

                            if(Me.ProfilePic == "/Content/images/default_image.png")
                            {

                            }
                            else
                            {
                                string[] myPreviousPic = Me.ProfilePic.Split('/');
                                string previousPrfPath = Path.Combine(Server.MapPath("~/Media"), myPreviousPic[myPreviousPic.Length - 1]);
                                System.IO.File.Delete(previousPrfPath);
                            }

                            Me.ProfilePic = "/Media/" + _FileName;

                            db.SaveChanges();
                        }


                    }
                }
                catch
                {

                }
                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult dashboard()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult fixtures()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var User = db.Users.Find((int)Session["Id"]);
                    var fixture = db.fixtures.ToList();

                    var fixtures = fixture.OrderBy(x => x.time);
                    var newFixture = fixtures.Where(f => f.time > DateTime.Now).ToList();


                    foreach (var fix in newFixture)
                    {
                        fix.HomeTeam = db.Teams.Find(fix.HomeTeamId);
                        fix.AwayTeam = db.Teams.Find(fix.AwayTeamId);
                    }

                    UserFixtureView ufv = new UserFixtureView();
                    ufv.myProfile = User;
                    ufv.fixtures = newFixture;

                    return View(ufv);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult friends()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var id = Session["Id"];
                    var MyFriends = db.Friends.ToList().Where(friend => friend.SenderFriendID1 == (int)Session["Id"] || friend.RecieverFriendID2 == (int)Session["Id"]).ToList();

                    foreach (var friend in MyFriends)
                    {
                        if (friend.SenderFriendID1 == (int)Session["Id"])
                        {
                            friend.RecieverFriend2 = db.Users.Find(friend.RecieverFriendID2);
                        }
                        else
                        {
                            friend.SenderFriend1 = db.Users.Find(friend.SenderFriendID1);
                        }
                    }

                    return View(MyFriends);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult teams()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var User = db.Users.Find((int)Session["Id"]);
                    var teams = db.Teams.ToList();

                    UserTeamView USerTeamView = new UserTeamView();
                    USerTeamView.myProfile = User;
                    USerTeamView.teams = teams;
                    return View(USerTeamView);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ViewTeam(int id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var User = db.Users.Find((int)Session["Id"]);
                    var team = db.Teams.Find(id);
                    var players = db.players.Where(p => p.TeamID == id).ToList();

                    UserPlayerView upv = new UserPlayerView();
                    upv.myProfile = User;
                    upv.team = team;
                    upv.players = players;

                    return View(upv);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddPost(String post, HttpPostedFileBase new_file)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                try
                {
                    if (new_file != null)
                    {
                        string _FileName = Path.GetFileName(new_file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Media"), _FileName);
                        new_file.SaveAs(_path);

                        using (MyDatabase db = new MyDatabase())
                        {
                            Post newPost = new Post((int)Session["Id"], post, "/Media/" + _FileName, DateTime.Now);

                            db.Posts.Add(newPost);

                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        using (MyDatabase db = new MyDatabase())
                        {
                            Post newPost = new Post((int)Session["Id"], post, "", DateTime.Now);

                            db.Posts.Add(newPost);

                            db.SaveChanges();
                        }
                    }
                }
                catch
                {

                }

                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SendFriendRequest(int id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    FriendRequest friendRequest = new FriendRequest((int)Session["Id"], id);
                    //friendRequest.Sender = db.Users.Find((int)Session["Id"]);
                    //friendRequest.Reciever = db.Users.Find(id);

                    db.FriendRequests.Add(friendRequest);

                    db.SaveChanges();
                }

                return RedirectToAction("index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Requests()
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                using (MyDatabase db = new MyDatabase())
                {
                    var AllRequests = db.FriendRequests.ToList().Where(Request => Request.RecieverID == (int)Session["Id"]);
                    foreach (var request in AllRequests)
                    {
                        request.Sender = db.Users.Find(request.SenderID);
                        request.Reciever = db.Users.Find(request.RecieverID);
                    }

                    return View(AllRequests);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult AcceptRequest(string id, string confirm, string f_id)
        {
            if (Session["Id"] != null && (bool)Session["Admin"] == false)
            {
                if (confirm == "1")
                {
                    using (MyDatabase db = new MyDatabase())
                    {
                        db.FriendRequests.Remove(db.FriendRequests.Find(Int16.Parse(f_id)));

                        db.Friends.Add(new Friends(Int16.Parse(id), (int)Session["Id"]));

                        db.SaveChanges();
                    }
                }
                else
                {
                    using (MyDatabase db = new MyDatabase())
                    {
                        db.FriendRequests.Remove(db.FriendRequests.Find(Int16.Parse(f_id)));

                        db.SaveChanges();
                    }
                }

                return RedirectToAction("friends");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

