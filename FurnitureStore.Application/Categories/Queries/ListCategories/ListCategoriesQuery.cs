﻿using ErrorOr;
using FurnitureStore.Domain.Categories;
using MediatR;

namespace FurnitureStore.Application.Categories.Queries.ListCategories;

public record ListCategoriesQuery : IRequest<ErrorOr<List<Category>>>;

