using ChatApp.Models.Dto;
using ChatApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatApp.Controllers
{
    public class SubjectController : Controller
    {
        ChatDbcontext db = new ChatDbcontext();

       

        [HttpGet]
        public ActionResult GetSubject(int? id)
        {
            Subject sub = db.Subjects.FirstOrDefault(s => s.Id == id);
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));

            if (sub == null)
            {
                return RedirectToAction("Index");
            }
            List<PostDto> listPostDto = db.Posts.Where(s => s.SubjectId == sub.Id)
                .Select(s => new PostDto
                {
                    Myavatar = user.Avatar,
                    TimePost = s.CreatedDate,
                    LikeNumber = s.LikeNumber,
                    NameOfUser = s.User.Name,
                    PostText = s.Text,
                    avatar = s.User.Avatar,
                    PostId = s.Id,
                    listComment = db.Comments.Where(k => k.PostId == s.Id)
                    .Select(k => new CommentDto
                    {
                        LikeNumber = k.LikeNumber,
                        NameOfUser = k.User.Name,
                        Text = k.Text,
                        Avatar = k.User.Avatar,
                        CommentId = k.Id
                    }).ToList()
                }).OrderByDescending(s => s.PostId).ToList();

            ViewBag.photo = sub.Photo;
            ViewBag.name = sub.Name;

            return View(listPostDto);
        }

        [HttpPost]
        public ActionResult SavePost(PostDto postDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Subject subject = db.Subjects.FirstOrDefault(us => us.Name.Equals(postDto.GroupName));
            Post post = new Post();
            post.LikeNumber = 0;
            post.SubjectId = subject.Id;
            post.Text = postDto.PostText;
            post.UserId = user.Id;
            post.CreatedDate = postDto.TimePost;
            db.Posts.Add(post);
            db.SaveChanges();
            int Postid = db.Posts.Max(s => s.Id);
            PostDto result = new PostDto
            {
                TimePost = postDto.TimePost,
                LikeNumber = postDto.LikeNumber,
                NameOfUser = user.Name,
                PostText = postDto.PostText,
                avatar = user.Avatar,
                PostId = Postid
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveComment(CommentDto commentDto)
        {
            var userName = Session["userName"] as string;
            User user = db.Users.FirstOrDefault(us => us.UserName.Equals(userName));
            Comment comment = new Comment
            {
                Text = commentDto.Text,
                LikeNumber = 0,
                PostId = commentDto.PostId,
                UserId = user.Id
            };
            db.Comments.Add(comment);
            db.SaveChanges();
            int CommentId = db.Comments.Max(s => s.Id);
            CommentDto result = new CommentDto
            {
                LikeNumber = 0,
                NameOfUser = user.Name,
                Text = commentDto.Text,
                Avatar = user.Avatar,
                CommentId = CommentId
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
