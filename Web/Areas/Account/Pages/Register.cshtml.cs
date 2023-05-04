using System.Diagnostics.CodeAnalysis;
using Application.Features.BlogUser;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Account.Pages;

public abstract class MediatorRegisterModel : PageModel
{   
    [BindProperty]
    public RegisterCommand Input { get; set; } = default!;
    
    public string? ReturnUrl { get; set; }
    
    [TempData]
    public string? ErrorMessage { get; set; }
    
    public virtual Task OnGetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
    
    public virtual Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null) => throw new NotImplementedException();
}

internal sealed class MediatorRegisterModel<TUser> : MediatorRegisterModel where TUser : class
{
    private readonly IMediator _mediator;

    public MediatorRegisterModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        await HttpContext.SignOutAsync();
        
        ReturnUrl = returnUrl;
    }
    
    public override async Task<IActionResult> OnPostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Bad Model...");
        }

        if (ModelState.IsValid)
        {
            var test = await _mediator.Send(Input);
            Console.WriteLine(test);
            Console.WriteLine("testing");
        }
        return Page();
    }
}
