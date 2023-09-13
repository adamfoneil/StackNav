using Microsoft.AspNetCore.Components;

namespace StackNav.Blazor.Services;

public class BreadcrumbNavigation
{
	private readonly Stack<(string Text, string Url)> Path = new();
	private readonly NavigationManager NavigationManager;

	public event Action? PathModified;

	public BreadcrumbNavigation(NavigationManager navigationManager)
	{
		NavigationManager = navigationManager;
	}

	public bool AllowGoBack => Path.Count > 1;

	public IEnumerable<string> PathNames => Path.Select(item => item.Text);

	public void Push(string text, string url)
	{
		Path.Push((text, url));
		PathModified?.Invoke();
	}

	public void GoTo(string text, string url)
	{
		Push(text, url);
		NavigationManager.NavigateTo(url, forceLoad: true);
	}
  
	public void GoBack()
	{
		var navEntry = Path.Pop();
		NavigationManager.NavigateTo(navEntry.Url);
	}
}
