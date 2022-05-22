namespace MyCocktailsApi.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MyCocktailsApi.Controllers;
    using MyCocktailsApi.Data;
    using MyCocktailsApi.Infrastructure;
    using MyCocktailsApi.Models;
    using MyCocktailsApi.Services;
    using Xunit;

    public class CockatilsControllerTests
    {
        private readonly Mock<ICocktailService> serviceStub = new();
        private readonly Mock<IMapper> mockedMapper = new();
        private readonly IMapper mapper;
        private readonly Random rand = new();

        public CockatilsControllerTests()
        {
            if (mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mainMapper = mappingConfig.CreateMapper();
                mapper = mainMapper;
            }
        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsCollectionIsNull_ReturnsNotFound()
        {
            serviceStub.Setup(s => s.GetAllAsync()).ReturnsAsync((IEnumerable<OutputCocktailModel>)null);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetAll();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsCollectionIsEmpty_ReturnsNotFound()
        {
            var emptyList = new List<OutputCocktailModel>();
            serviceStub.Setup(s => s.GetAllAsync()).ReturnsAsync(emptyList);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetAll();

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsExist_ReturnsAllCocktails()
        {
            var cocktailList = GetGocktails(3);
            serviceStub.Setup(s => s.GetAllAsync()).ReturnsAsync(cocktailList);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetAll();
            var dtos = ((result as OkObjectResult).Value) as IList<OutputCocktailModel>;

            result.Should().BeOfType<OkObjectResult>();
            dtos.Should().BeEquivalentTo(cocktailList,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailById_WhenIdIsNull_ReturnsNotFound()
        {
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetById(null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailById_WhenCocktailIsNull_ReturnsNotFound()
        {
            serviceStub.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((OutputCocktailModel)null);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetById(Guid.NewGuid().ToString());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailById_WhenCocktailsExist_ReturnsExpectedCocktail()
        {
            var expectedCocktail = GetRandomCocktail();
            serviceStub.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(expectedCocktail);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetById(Guid.NewGuid().ToString());
            var dto = ((result as OkObjectResult).Value) as OutputCocktailModel;

            result.Should().BeOfType<OkObjectResult>();
            dto.Should().BeEquivalentTo(expectedCocktail,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailByName_WhenNameIsNull_ReturnsNotFound()
        {
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByName(null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailByName_WhenCocktailIsNull_ReturnsNotFound()
        {
            serviceStub.Setup(s => s.GetByNameAsync(It.IsAny<string>())).ReturnsAsync((OutputCocktailModel)null);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByName(Guid.NewGuid().ToString());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailByName_WhenCocktailsExist_ReturnsExpectedCocktail()
        {
            var expectedCocktail = GetRandomCocktail();
            serviceStub.Setup(s => s.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(expectedCocktail);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByName(Guid.NewGuid().ToString());
            var dto = ((result as OkObjectResult).Value) as OutputCocktailModel;

            result.Should().BeOfType<OkObjectResult>();
            dto.Should().BeEquivalentTo(expectedCocktail,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCategoryIsNull_ReturnsNotFound()
        {
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByCategoryName(null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionIsNull_ReturnsNotFound()
        {
            serviceStub.Setup(s => s.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync((IEnumerable<OutputCocktailModel>)null);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByCategoryName(Guid.NewGuid().ToString());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionIsEmpty_ReturnsNotFound()
        {
            var emptyList = new List<OutputCocktailModel>();
            serviceStub.Setup(s => s.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(emptyList);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByCategoryName(Guid.NewGuid().ToString());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionExist_ReturnsCollection()
        {
            var cocktailList = GetGocktails(2);
            cocktailList[1].Category = cocktailList[0].Category;

            serviceStub.Setup(s => s.GetByCategoryAsync(cocktailList[0].Category)).ReturnsAsync(cocktailList);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetByCategoryName(cocktailList[0].Category);
            var dtos = ((result as OkObjectResult).Value) as IList<OutputCocktailModel>;

            result.Should().BeOfType<OkObjectResult>();
            dtos.Should().BeEquivalentTo(cocktailList,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task CreateCocktail_WithNullName_ReturnsBadRequest()
        {
            var modelCocktail = GetRandomCocktail();
            var inputModel = mapper.Map<InputCocktailModel>(modelCocktail);
            inputModel.Name = null;
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);
            controller.ModelState.AddModelError("", "Name is required!");

            var result = await controller.Create(inputModel);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task CreateCocktail_WithValidModel_ReturnsCreatedCocktail()
        {
            var modelCocktail = GetRandomCocktail();
            var inputModel = mapper.Map<InputCocktailModel>(modelCocktail);
            var inputServiceModel = mapper.Map<InputCocktailServiceModel>(inputModel);

            serviceStub.Setup(s => s.CreateAsync(It.IsAny<InputCocktailServiceModel>())).ReturnsAsync(modelCocktail);
            mockedMapper
                .Setup(m => m.Map<InputCocktailServiceModel>(It.IsAny<InputCocktailModel>()))
                .Returns(mapper.Map<InputCocktailServiceModel>(inputModel));

            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.Create(inputModel);
            serviceStub.Verify(s => s.CreateAsync(It.IsAny<InputCocktailServiceModel>()), Times.Exactly(1));

            result.Should().BeOfType<OkObjectResult>();
            var dto = (result as OkObjectResult).Value as OutputCocktailModel;

            dto.Should().BeEquivalentTo(inputModel,
                options => options.ComparingByMembers<OutputCocktailModel>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task LikeCocktail_WhenIdIsNull_ReturnsNotFound()
        {
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.Like(null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task LikeCocktail_WhenCocktailIsNull_ReturnsNotFound()
        {
            serviceStub.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((OutputCocktailModel)null);
            var controller = new CocktailsController(serviceStub.Object, mockedMapper.Object);

            var result = await controller.GetById(Guid.NewGuid().ToString());

            result.Should().BeOfType<NotFoundResult>();
        }














        private IList<OutputCocktailModel> GetGocktails(int count = 0)
        {
            var cocktailList = new List<OutputCocktailModel>();
            for (int i = 0; i < count; i++)
            {
                var cocktail = GetRandomCocktail(i.ToString());
                cocktailList.Add(cocktail);
            }

            return cocktailList;
        }

        private OutputCocktailModel GetRandomCocktail(string suffix = "")
        {
            var cocktail = new OutputCocktailModel();
            cocktail.Id = Guid.NewGuid().ToString();
            cocktail.Name = ($"CocktailName {suffix}").TrimEnd();
            cocktail.Category = ($"CategoryName {suffix}").TrimEnd();
            cocktail.Instructions = GetRandomInstructions(suffix);
            cocktail.Likes = rand.Next(0, 10);
            cocktail.UsersLike = GetUserLikes(cocktail.Likes);
            cocktail.Glass = GetRandomGlassType();
            string imageNumber = string.IsNullOrEmpty(suffix) ? "1" : suffix;
            cocktail.PictureUrl = "https://MyRandomImages/" + imageNumber + ".jpg";

            cocktail.Ingredients = GetRandomIngredients();
            cocktail.DateModified = DateTime.UtcNow;

            return cocktail;
        }

        private string GetRandomGlassType()
        {
            Array values = Enum.GetValues(typeof(GlassType));
            var randomGlassType = (GlassType)values.GetValue(rand.Next(values.Length));

            switch (randomGlassType)
            {
                case GlassType.OldFashioned:
                    return "Old-fashioned";
                case GlassType.CopperMug:
                    return "Copper Mug";
                default:
                    return randomGlassType.ToString();
            }
        }

        private string GetRandomInstructions(string suffix = "")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append($"Those are my instructions {i} {suffix}. ");
            }

            return sb.ToString().Trim();
        }

        private IList<OutputIngredientModel> GetRandomIngredients()
        {
            var ingredientsList = new List<OutputIngredientModel>();

            var ingredientsCount = rand.Next(1, 5);

            for (int i = 0; i < ingredientsCount; i++)
            {
                var ingredient = new OutputIngredientModel()
                {
                    Name = Guid.NewGuid().ToString(),
                    Quantity = Guid.NewGuid().ToString()
                };

                ingredientsList.Add(ingredient);
            }

            return ingredientsList;
        }

        private IList<string> GetUserLikes(int count)
        {
            var userLikesCollection = new List<string>();
            for (int i = 0; i < count; i++)
            {
                userLikesCollection.Add(Guid.NewGuid().ToString());
            }

            return userLikesCollection;
        }
    }
}
