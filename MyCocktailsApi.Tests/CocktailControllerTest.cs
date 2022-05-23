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

    using static ApiConstants.Cocktail;

    public class CockatilsControllerTests
    {
        private readonly Mock<ICocktailService> mockedService = new();
        private readonly Mock<IMapper> mockedMapper = new();
        private readonly IMapper mapper;
        private readonly Random rand = new();
        private readonly CocktailsController controller;


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

            this.controller = new CocktailsController(mockedService.Object, mockedMapper.Object);

        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsCollectionIsNull_ReturnsNotFound()
        {
            //Arrange
            mockedService.Setup(s => s.GetAllAsync()).ReturnsAsync(() => null);

            //Act
            var result = await controller.GetAll();

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingOrEmptyCollectionMessage);
        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsCollectionIsEmpty_ReturnsNotFound()
        {
            //Arrange
            var emptyList = new List<OutputCocktailModel>();
            mockedService.Setup(s => s.GetAllAsync()).ReturnsAsync(emptyList);

            //Act
            var result = await controller.GetAll();

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingOrEmptyCollectionMessage);
        }

        [Fact]
        public async Task GetAllCocktails_WhenCocktailsExist_ReturnsAllCocktails()
        {
            //Arrange
            var cocktailList = GetGocktails(3);
            mockedService.Setup(s => s.GetAllAsync()).ReturnsAsync(cocktailList);

            //Act
            var result = await controller.GetAll();
            var dtos = ((result as OkObjectResult).Value) as IList<OutputCocktailModel>;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            dtos.Should().BeEquivalentTo(cocktailList,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailById_WhenIdIsNull_ReturnsBadRequest()
        {
            //Arrange
            string cocktailId = null;

            //Act
            var result = await controller.GetById(cocktailId);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(RequiredIdMessage);
        }

        [Fact]
        public async Task GetCocktailById_WhenNotExistingCocktail_ReturnsNotFound()
        {
            //Arrange
            mockedService.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var result = await controller.GetById(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingCocktailMessage);
        }

        [Fact]
        public async Task GetCocktailById_WhenCocktailsExist_ReturnsExpectedCocktail()
        {
            //Arrange
            var expectedCocktail = GetRandomCocktail();
            mockedService.Setup(s => s.GetByIdAsync(expectedCocktail.Id)).ReturnsAsync(expectedCocktail);

            //Act
            var result = await controller.GetById(expectedCocktail.Id);
            var dto = ((result as OkObjectResult).Value) as OutputCocktailModel;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            dto.Should().BeEquivalentTo(expectedCocktail,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailByName_WhenNameIsNull_ReturnsBadRequest()
        {
            //Arrange
            string CocktailName = null;

            //Act
            var result = await controller.GetByName(CocktailName);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(RequriedNameMessage);
        }

        [Fact]
        public async Task GetCocktailByName_WhenNotExistingCocktail_ReturnsNotFound()
        {
            //Arrange
            var cocktailName = Guid.NewGuid().ToString();
            mockedService.Setup(s => s.GetByNameAsync(cocktailName)).ReturnsAsync(() => null);

            //Act
            var result = await controller.GetByName(cocktailName);
            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(string.Format(NotExistingCocktailWithNameMessage, cocktailName));
        }

        [Fact]
        public async Task GetCocktailByName_WhenCocktailsExist_ReturnsExpectedCocktail()
        {
            //Arrange
            var expectedCocktail = GetRandomCocktail();
            mockedService.Setup(s => s.GetByNameAsync(expectedCocktail.Name)).ReturnsAsync(expectedCocktail);

            //Act
            var result = await controller.GetByName(expectedCocktail.Name);
            var dto = ((result as OkObjectResult).Value) as OutputCocktailModel;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            dto.Should().BeEquivalentTo(expectedCocktail,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCategoryIsNull_ReturnsBadRequest()
        {
            //Arrange
            string cocktailCategory = null;

            //Act
            var result = await controller.GetByCategoryName(cocktailCategory);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(RequiredCategoryMessage);
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionIsNull_ReturnsNotFound()
        {
            //Arrange
            mockedService.Setup(s => s.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var result = await controller.GetByCategoryName(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingOrEmptyCollectionMessage);
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionIsEmpty_ReturnsNotFound()
        {
            //Arrange
            var emptyList = new List<OutputCocktailModel>();
            mockedService.Setup(s => s.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(emptyList);

            //Act
            var result = await controller.GetByCategoryName(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingOrEmptyCollectionMessage);
        }

        [Fact]
        public async Task GetCocktailsByCategory_WhenCocktailsCollectionExist_ReturnsCollection()
        {
            //Arrange
            var cocktailList = GetGocktails(2);
            cocktailList[1].Category = cocktailList[0].Category;
            mockedService.Setup(s => s.GetByCategoryAsync(cocktailList[0].Category)).ReturnsAsync(cocktailList);

            //Act
            var result = await controller.GetByCategoryName(cocktailList[0].Category);
            var dtos = ((result as OkObjectResult).Value) as IList<OutputCocktailModel>;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            dtos.Should().BeEquivalentTo(cocktailList,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task CreateCocktail_WithNullName_ReturnsBadRequest()
        {
            //Arrange
            var modelCocktail = GetRandomCocktail();
            var inputModel = mapper.Map<InputCocktailModel>(modelCocktail);
            inputModel.Name = null;
            controller.ModelState.AddModelError(string.Empty, RequriedNameMessage);

            //Act
            var result = await controller.Create(inputModel);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(InvalidModelMessage);
        }

        [Fact]
        public async Task CreateCocktail_WithValidModel_ReturnsCreatedCocktail()
        {
            //Arrange
            var modelCocktail = GetRandomCocktail();
            var inputModel = mapper.Map<InputCocktailModel>(modelCocktail);
            var inputServiceModel = mapper.Map<InputCocktailServiceModel>(inputModel);
            mockedService.Setup(s => s.CreateAsync(inputServiceModel)).ReturnsAsync(modelCocktail);
            mockedMapper.Setup(m => m.Map<InputCocktailServiceModel>(inputModel)).Returns(inputServiceModel);

            //Act
            var result = await controller.Create(inputModel);
            var dto = (result as OkObjectResult).Value as OutputCocktailModel;

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            mockedService.Verify(s => s.CreateAsync(inputServiceModel), Times.Exactly(1));
            dto.Should().BeEquivalentTo(inputModel,
                options => options.ComparingByMembers<OutputCocktailModel>());
        }

        [Fact]
        public async Task LikeCocktail_WhenNotLoggedIn_ReturnsBadRequest()
        {
            //Arrange
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => string.Empty
            };

            //Act
            var result = await testController.Like(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(UnauthorizedMessage);
        }

        [Fact]
        public async Task LikeCocktail_WhenIdIsNull_ReturnsBadRequest()
        {
            //Arrange
            string cocktailId = null;
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };

            //Act
            var result = await testController.Like(cocktailId);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(RequiredIdMessage);
        }

        [Fact]
        public async Task LikeCocktail_WhenCocktailIsNull_ReturnsNotFound()
        {
            //Arrange
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };

            mockedService.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var result = await testController.Like(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingCocktailMessage);
        }

        [Fact]
        public async Task LikeCocktail_WhenCocktailExistl_UpdateLikesSuccessfully()
        {
            //Arrange
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };

            var cocktail = GetRandomCocktail();
            mockedService.Setup(s => s.GetByIdAsync(cocktail.Id)).ReturnsAsync(cocktail);
            mockedService.Setup(s => s.UpdateLikes(cocktail, TestUserEmail));

            //Act
            var result = await testController.Like(cocktail.Id);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(UpdatedLikesMessage);
        }

        [Fact]
        public async Task UpdateCocktail_WhenNotLoggedIn_ReturnsBadRequest()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var updatedCocktail = GetRandomCocktail();
            var updatedCocktailInputModel = this.mapper.Map<InputCocktailModel>(updatedCocktail);

            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => string.Empty
            };

            //Act
            var result = await testController.Update(cocktail.Id, updatedCocktailInputModel);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(UnauthorizedMessage);
        }

        [Fact]
        public async Task UpdateCocktail_WithNullName_ReturnsBadRequest()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var updatedCocktail = GetRandomCocktail();
            var updatedCocktailInputModel = mapper.Map<InputCocktailModel>(updatedCocktail);
            updatedCocktailInputModel.Name = null;
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };

            testController.ModelState.AddModelError(cocktail.Name, RequriedNameMessage);

            //Act
            var result = await testController.Update(cocktail.Id, updatedCocktailInputModel);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(InvalidModelMessage);
        }

        [Fact]
        public async Task UpdateCocktail_WhenNotExisting_ReturnsNotFound()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var updatedCocktail = GetRandomCocktail();
            var updatedCocktailInputModel = mapper.Map<InputCocktailModel>(updatedCocktail);
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };
            mockedService.Setup(s => s.GetByIdAsync(cocktail.Id)).ReturnsAsync(() => null);

            //Act
            var result = await testController.Update(cocktail.Id, updatedCocktailInputModel);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingCocktailMessage);
        }

        [Fact]
        public async Task UpdateCocktail_WhenExisting_ReturnsConfirmMessage()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var updatedCocktail = GetRandomCocktail();
            var updatedCocktailInputModel = mapper.Map<InputCocktailModel>(updatedCocktail);
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };
            mockedService.Setup(s => s.GetByIdAsync(cocktail.Id)).ReturnsAsync(cocktail);
            mockedService.Setup(s => s.UpdateAsync(cocktail, updatedCocktailInputModel));

            //Act
            var result = await testController.Update(cocktail.Id, updatedCocktailInputModel);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(UpdatedCocktailMessage);
            mockedService.Verify(s => s.UpdateAsync(cocktail, updatedCocktailInputModel), Times.Exactly(1));
        }

        [Fact]
        public async Task DeleteCocktail_WhenNotLoggedIn_ReturnsBadRequest()
        {
            //Arrange
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => string.Empty
            };

            //Act
            var result = await testController.Delete(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(UnauthorizedMessage);
        }


        [Fact]
        public async Task DeleteCocktail_WithNullId_ReturnsBadRequest()
        {
            //Arrange
            string cocktailId = null;
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };

            //Act
            var result = await testController.Delete(cocktailId);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(RequiredIdMessage);
        }

        [Fact]
        public async Task DeleteCocktail_WhenNotExisting_ReturnsNotFound()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };
            mockedService.Setup(s => s.GetByIdAsync(cocktail.Id)).ReturnsAsync(() => null);

            //Act
            var result = await testController.Delete(cocktail.Id);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(NotExistingCocktailMessage);
        }

        [Fact]
        public async Task DeleteCocktail_WhenExisting_ReturnsConfirmMessage()
        {
            //Arrange
            var cocktail = GetRandomCocktail();
            var testController = new CocktailsController(mockedService.Object, mockedMapper.Object)
            {
                GetUserId = () => TestUserEmail
            };
            mockedService.Setup(s => s.GetByIdAsync(cocktail.Id)).ReturnsAsync(cocktail);
            mockedService.Setup(s => s.DeleteAsync(cocktail.Id));

            //Act
            var result = await testController.Delete(cocktail.Id);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            (result as ObjectResult).Value.Should().BeEquivalentTo(DeletedCocktailMessage);
            mockedService.Verify(s => s.DeleteAsync(cocktail.Id), Times.Exactly(1));
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
