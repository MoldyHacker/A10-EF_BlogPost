using System;
using Castle.Core.Internal;
using EFTutorial.Models;
using Microsoft.Extensions.Logging;

namespace ApplicationTemplate.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly ILogger<IMainService> _logger;
    public MainService(ILogger<IMainService> logger)
    {
        _logger = logger;
    }

    public void Invoke()
    {
        string choice;
        do
        {
            Console.Write("\n1) Display Blogs" +
                          "\n2) Add Blog" +
                          "\n3) Display Posts" +
                          "\n4) Add Post" +
                          "\n5) Exit" +
                          "\n> ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nDisplaying all blogs...");
                    int numOfBlogs = ListBlogs().Count;
                    Console.Write(numOfBlogs);
                    Console.WriteLine(numOfBlogs > 1 ? " Blogs returned" : " Blog returned");
                    ListBlogs().ForEach(Console.WriteLine);
                    break;


                case "2":
                    Console.Write("\nName of blog: ");
                    string newBlogName = Console.ReadLine() ?? string.Empty;
                    if (newBlogName.IsNullOrEmpty())
                    {
                        _logger.LogError("Blog name cannot be null");
                        break;
                    }

                    var newBlog = new Blog();
                    newBlog.Name = newBlogName;

                    using (var db = new BlogContext())
                    {
                        db.Blogs.Add(newBlog);
                        db.SaveChanges();
                    }
                    Console.WriteLine($"Blog added - \"{newBlog.Name}\"");
                    break;


                case "3":
                    Console.WriteLine("\nWhich blog would you like to display the posts from?");
                    ListBlogs().ForEach(Console.WriteLine);

                    Console.Write("\nPlease enter the blog ID(0 to display all posts): ");
                    string userBlogPostString = Console.ReadLine();

                    // validate and set blog id
                    if (IdConvert(userBlogPostString).Equals(-1))
                        break;
                    int userBlogPostChoice = IdConvert(userBlogPostString);


                    using (var db = new BlogContext())
                    {
                        var chosenBlog = db.Blogs.FirstOrDefault(x => x.BlogId == userBlogPostChoice);
                        if (userBlogPostChoice == 0)
                            foreach (var dbPost in db.Posts)
                                Console.WriteLine($"Blog ID: {dbPost.BlogId}\tPost: {dbPost.PostId} Title: {dbPost.Title} Content: {dbPost.Content}");
                        else
                            foreach (var chosenBlogPost in chosenBlog.Posts)
                                Console.WriteLine($"Blog ID: {chosenBlogPost.BlogId}\tPost {chosenBlogPost.PostId} Title: {chosenBlogPost.Title} Content: {chosenBlogPost.Content}");
                    }
                    break;


                case "4":
                    Console.WriteLine("\nWhich blog would you like to post to?");
                    ListBlogs().ForEach(Console.WriteLine);
                    Console.Write("Please enter the blog ID: ");
                    string userBlogString = Console.ReadLine();

                    // convert and validate input
                    if (IdConvert(userBlogString).Equals(-1))
                        break;
                    int userBlogChoice = IdConvert(userBlogString);

                    // enter post title and validate
                    Console.Write("Enter post title: ");
                    string postTitle = Console.ReadLine();
                    if (postTitle.IsNullOrEmpty())
                    {
                        _logger.LogError("Post must have a title");
                        break;
                    }

                    // Enter post content
                    Console.Write("Enter the post content: ");
                    string postContent = Console.ReadLine();

                    // create post object and update values
                    var post = new Post();
                    post.BlogId = userBlogChoice;
                    post.Title = postTitle;
                    post.Content = postContent;

                    using (var db = new BlogContext())
                    {
                        db.Posts.Add(post);
                        db.SaveChanges();
                    }

                    Console.WriteLine($"Post {postTitle} added to blog {userBlogChoice} with content {postContent}");
                    break;

                case "5":
                    // exit
                    break;
                default:
                    _logger.LogInformation("Invalid command");
                    break;
            }
        }
        while (choice != "5");
    }

    private List<string> ListBlogs()
    {
        List<string> list = new List<string>();
        using (var db = new BlogContext())
            foreach (var b in db.Blogs)
                list.Add($"{b.BlogId}: {b.Name}");
        return list;
    }

    private bool IdValidate(string id)
    {
        int num = -1;
        if (id.IsNullOrEmpty() || !int.TryParse(id, out num))
        {
            _logger.LogError(id.IsNullOrEmpty()
                ? "ID cannot be null"
                : "ID must be a number");
            return false;
        }
        return true;
    }

    private int IdConvert(string id)
    {
        if (!IdValidate(id))
            return -1;
        int userBlogChoice = int.Parse(id);
        if (userBlogChoice > ListBlogs().Count || userBlogChoice < 0)
        {
            _logger.LogError("ID out of range");
            return -1;
        }
        return userBlogChoice;
    }
}
