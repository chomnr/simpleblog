using System.Net;
using System.Security.Claims;
using Application.Common;
using Application.Common.Interface;
using Application.Entities;
using Application.Features.BlogUser;
using Application.Features.Post;
using Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Application.Infrastructure.Services;

public class PostService : IPostService
{
    /* TODO: ADD CUSTOM ERRORS FOR CONVENIENCE */
    /* TODO: THROW CONSTANTS IN DIFFERENT FILE SO IT'S PERSISTENT. */
    
    private readonly DatabaseDbContext _context;
    
    public PostService(DatabaseDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreateAsync(CreatePostCommand command, string userId)
    {
        var title = command.Title;
        var body = WebUtility.HtmlDecode(command.Body);
        var tags = command.Tags;

        if (!PostConstraints.IsValidTitle(title)) { return false; }
        if (!PostConstraints.IsValidBody(body)) { return false; }
        if (!PostConstraints.AreTagsValid(tags)) { return false; }
        
        var posts = _context.Posts;
        var post = PostHelper.CreateSimplifiedPost(userId, title, body, tags);
        
        posts.Add(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<JsonResult> RetrieveAllAsync(RetrievePostsCommand command, bool partial)
    {
        var page = command.Page;
        //var retrieval = page == 1 ? postPerPage : postPerPage;
        const int retrieval = PostConstraints.RetrievalAmount;

        var posts = _context.Posts
            .Join(
                _context.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) => new { Post = post, UserName = user.UserName }
            ).OrderByDescending(post => post.Post.DateCreated)
            .Select(postWithUserName => new
            {
                postWithUserName.Post.Id,
                postWithUserName.Post.UserId,
                postWithUserName.Post.Title,
                postWithUserName.Post.Body,
                postWithUserName.Post.DateCreated,
                postWithUserName.Post.Tags,
                UserName = postWithUserName.UserName,
            });

        var skipCalc = page != 1 ? (page - 1) * retrieval : 0;
        var result = await posts.Skip(skipCalc).Take(retrieval).ToListAsync();
        var totalPosts = (int)Math.Ceiling((double)posts.Count() / retrieval);
        return new JsonResult(JsonConvert.SerializeObject(new
        {
            pageCount = totalPosts,
            AllPosts = result
        }));
    }

    public async Task<JsonResult> RetrieveSpecificAsync(RetrievePostCommand command)
    {
        var posts = _context.Posts;

        var id = command.Id;

        var result = await posts.FindAsync(id);
        var user = await _context.Users.FindAsync(result.UserId);
        result.Username = user != null ? user.UserName : "{DELETED_USER}";
        
        return new JsonResult(JsonConvert.SerializeObject(result));
    }

    public async Task<JsonResult> RetrieveAllFromUserAsync(ViewUserCommand command)
    {
        var posts = await _context.Posts
            .Where(u => u.UserId == command.Id)
            .Select(u => u).ToListAsync();
        return new JsonResult(JsonConvert.SerializeObject(posts));
    }

    public async Task<JsonResult> RetrieveAllByTag(RetrievePostsByTagCommand command)
    {
        var page = command.Page;
        const int retrieval = PostConstraints.RetrievalAmount;

        var posts = _context.Posts
            .Where(u => u.Tags.Contains(command.Tag))
            .Join(
                _context.Users,
                post => post.UserId,
                user => user.Id,
                (post, user) => new { Post = post, UserName = user.UserName }
            ).OrderByDescending(post => post.Post.DateCreated)
            .Select(postWithUserName => new
            {
                postWithUserName.Post.Id,
                postWithUserName.Post.UserId,
                postWithUserName.Post.Title,
                postWithUserName.Post.Body,
                postWithUserName.Post.DateCreated,
                postWithUserName.Post.Tags,
                UserName = postWithUserName.UserName,
            });
        
        var skipCalc = page != 1 ? (page - 1) * retrieval : 0;
        var result = await posts.Skip(skipCalc).Take(retrieval).ToListAsync();
        var totalPosts = (int)Math.Ceiling((double)posts.Count() / retrieval);
        
        return new JsonResult(JsonConvert.SerializeObject(new
        {
            PageCount = totalPosts,
            AllPosts = result
        }));
    }

    public async Task<bool> DeleteAsync(DeletePostCommand command, string userId, string role)
    {

        var posts = _context.Posts;
        var postId = command.PostId;
        var findPost = await posts.FindAsync(postId);

        if (findPost != null)
        {
            if (findPost.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
            {
                posts.Remove(findPost);
                await _context.SaveChangesAsync();
                return true;
            }

            if (role != null && role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                posts.Remove(findPost);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        return false;
    }

    public async Task<bool> EditAsync(EditPostCommand command)
    {
        var title = command.Title;
        var body = WebUtility.HtmlDecode(command.Body);
        var tags = command.Tags;

        var post = _context.Posts;
        var contextPost = await post.FindAsync(command.PostId);

        if (contextPost != null)
        {
            if (!string.IsNullOrEmpty(title) && !contextPost.Title.Equals(title))
            {
                if (!PostConstraints.IsValidTitle(title)) { return false; }
                contextPost.Title = title;
            }
            if (!contextPost.Body.Equals(body))
            {
                if (!PostConstraints.IsValidBody(body)) { return false; }
                contextPost.Body = body;
            }

            if (!contextPost.Tags.SequenceEqual(tags))
            {
                if (!PostConstraints.AreTagsValid(tags)) { return false; }
                contextPost.Tags = tags;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}