using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Demnoboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Demnoboard.Controllers;

public class PostsController : Controller
{
    private readonly IOptions<CaptchaConfig> _config;
    private readonly DbContext _db;
    private readonly ILogger<PostsController> _logger;

    public PostsController(DbContext databaseContext, IOptions<CaptchaConfig> config, ILogger<PostsController> logger)
    {
        _db = databaseContext;
        _config = config;
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewBag.Posts = Enumerable.Reverse(_db.Posts.ToList());
        return View();
    }

    public IActionResult Post(int id)
    {
        var post = _db.Posts.
            Include(repl => repl.Replies)
            .FirstOrDefault(e => e.Id == id);

        if (post is null)
            return View("404");
        
        ViewBag.post = post;
        
        return View();
    }
    
    public IActionResult CreatePost()
    {
        var captchaResponse = Request.Form["g-recaptcha-response"];
        if (!VerifyCaptcha.Verify(captchaResponse, _config.Value.ServerKey))
            return View("InputError");
        
        string text = Request.Form["Text"];
        string title = Request.Form["title"];
        
        if (text.Length > 115 || title.Length > 115)
            return View("InputError");
        
        var post = new Post { Title = Request.Form["Title"], Text = Request.Form["Text"]};
        _db.Posts.Add(post);
        
        if (_db.Posts.Count() > 100)
            _db.Posts.Remove(_db.Posts.First());
        
        _db.SaveChanges();
        return Redirect("~/");
    }
    
    public IActionResult CreateReply()
    {
        var captchaResponse = Request.Form["g-recaptcha-response"];
        if (!VerifyCaptcha.Verify(captchaResponse, _config.Value.ServerKey))
            return View("InputError");
        
        int id = Convert.ToInt32(Request.Form["id"]);
        string title = Request.Form["title"];
        string text = Request.Form["text"];
        
        if (text.Length > 115 || title.Length > 115)
            return View("InputError");
        
        _db.Posts.
            Include(repl => repl.Replies)
            .FirstOrDefault(e => e.Id == id)
            ?.Replies?.Add(new Reply {Text = text, Title = title});

        var posts = _db.Posts.
            Include(repl => repl.Replies)
            .FirstOrDefault(e => e.Id == id)!.Replies;
        if (posts != null && posts.Count > 50)
            _db.Remove(_db.Posts.FirstOrDefault(e => e.Id == id));
        
        _db.SaveChanges();
        return Redirect("~/Posts/Post?id="+id);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}