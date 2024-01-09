﻿namespace MRA.Identity.ClientWebApp.Services.ContentService;

public interface IContentService
{
    string this[string name] { get; }
    public Task ChangeCulture(string name);
    public Task<string> GetCurrentCulture();
    public Task InitializeCultureAsync();
}