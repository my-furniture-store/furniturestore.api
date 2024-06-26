﻿using FurnitureStore.Contracts.Categories;
using FurnitureStore.Domain.Categories;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureStore.API.Tests.Integration.Controllers.CategoriesController;

[Collection("FurnitureStore.API Collection")]
public class GetCategoriesControllerTests : IAsyncLifetime
{
    private readonly FurnistoreApiFactory _appFactory;
    private readonly HttpClient _httpClient;    

    public GetCategoriesControllerTests(FurnistoreApiFactory appFactory)
    {
        _appFactory = appFactory;
        _httpClient = appFactory.CreateClient();
    }


    [Fact]
    public async Task Get_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync($"api/categories/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await HttpResponseHelper.ReadFromResponse<ValidationProblemDetails>(response);

        problem!.Title.Should().Be("Not Found");
        problem.Status.Should().Be(404);
    }


    [Fact]
    public async Task Get_ReturnsCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category(name:"Chairs",id: Guid.NewGuid());
        await CategoryTestHelper.CreateCategory(_appFactory, category);

        // Act
        var response = await _httpClient.GetAsync($"api/categories/{category.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoryResponse = await HttpResponseHelper.ReadFromResponse<CategoryResponse>(response);
        categoryResponse!.Should().NotBeNull();
        categoryResponse!.Should().BeOfType<CategoryResponse>();
        categoryResponse!.Name.Should().Be(category.Name);
        categoryResponse!.Id.Should().Be(category.Id);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await CategoryTestHelper.ClearAllCategories(_appFactory);
    }
}
