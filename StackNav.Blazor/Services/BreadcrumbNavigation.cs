using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Distributed;
using StackNav.Blazor.Extensions;

namespace StackNav.Blazor.Services;

public class BreadcrumbNavigation
{
	private readonly Stack<NavEntry> Path = new();
	private readonly NavigationManager NavigationManager;
	private readonly IDistributedCache Cache;

	public event Action? PathModified;

	private const string CacheKey = "stack";

	public BreadcrumbNavigation(NavigationManager navigationManager, IDistributedCache cache)
	{
		NavigationManager = navigationManager;
		Cache = cache;
		Path = cache.GetItem<Stack<NavEntry>>(CacheKey) ?? new();
	}

	public bool AllowGoBack => Path.Count > 1;

	public IEnumerable<string> PathNames => Path.Select(item => item.Text);

	public void Push(string text, string url)
	{
		Path.Push(new(text, url));
		Cache.SetItem(CacheKey, Path);
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

	private class NavEntry
	{
        public NavEntry()
        {				
        }

        public NavEntry(string text, string url)
        {
			Text = text;
			Url = url;
        }

        public string Url { get; set; } = default!;
		public string Text { get; set; } = default!;
	}
}
