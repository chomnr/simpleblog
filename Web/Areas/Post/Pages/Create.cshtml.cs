using System.Diagnostics.CodeAnalysis;
using System.Text;
using Application.Entities;
using Application.Features.Post;
using Ganss.Xss;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Web.Areas.Post.Pages;

public class MediatorCreatePostModel : PageModel
{
    [BindProperty]
    public CreatePostCommand Input { get; set; } = default!;
    
    [BindProperty]
    public string TagHolder { get; set; }

    public string? ReturnUrl { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }
    
    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorCreatePostModel<TPost> : MediatorCreatePostModel where TPost : class
{
    private readonly IMediator _mediator;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MediatorCreatePostModel(IMediator mediator, SignInManager<BlogUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {   
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        if (!_signInManager.IsSignedIn(_httpContextAccessor.HttpContext.User))
        {
            Response.Redirect("/account/login");
        }
        else
        {
            returnUrl ??= Url.Content("~/");
        }

        ReturnUrl = returnUrl;
    }
    
    public override async Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var sanitizer = new HtmlSanitizer();
            // Converts {"[\"yes\",\"hello\"]"} to {dog,yes,hello,readable} in Database.
             //Input.Body = Convert.ToBase64String(Encoding.ASCII.GetBytes(Input.Body));
            if (TagHolder != null) {
                Input.Tags = JsonConvert.DeserializeObject<List<string>>(TagHolder.ToUpper());
            }
            
            Input.Title = sanitizer.Sanitize(Input.Title);
            Input.Body = sanitizer.Sanitize(Input.Body);
            
            var result = await _mediator.Send(Input);
            if (result)
            {
                Response.Redirect("/post/create/confirmation");
            }
        }
        return Page();
    }
}