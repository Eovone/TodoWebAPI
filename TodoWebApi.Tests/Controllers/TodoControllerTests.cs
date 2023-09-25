using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoWebApi.Controllers;
using Moq;
using Entity;

namespace TodoWebApi.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TodoController>> _loggerMock;

        public TodoControllerTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TodoController>>();
        }

        [Fact]
        public async Task TodoController_GetAllTodos_WithData_Return200()
        {
            var fakeTodos = new List<TodoDbModel>();
            _todoRepositoryMock.Setup(repo => repo.GetAllTodos(0)).ReturnsAsync(fakeTodos);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetAllTodos(0);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_GetAllTodos_NoData_Return404()
        {
            _todoRepositoryMock.Setup(repo => repo.GetAllTodos(It.IsAny<int>())).ReturnsAsync((List<TodoDbModel>)null);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetAllTodos(0);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_GetAllTodos_WithError_Return500()
        {
            _todoRepositoryMock.Setup(repo => repo.GetAllTodos(It.IsAny<int>())).ThrowsAsync(new Exception("Test Exception"));
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetAllTodos(0);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_GetTodo_WithData_Return200()
        {
            var fakeTodo = new TodoDbModel{ Id = 1 };
            _todoRepositoryMock.Setup(repo => repo.GetTodo(fakeTodo.Id)).ReturnsAsync(fakeTodo);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetTodo(fakeTodo.Id);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_GetTodo_NoData_Return404()
        {
            _todoRepositoryMock.Setup(repo => repo.GetTodo(1)).ReturnsAsync((TodoDbModel)null);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetTodo(1);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_GetTodo_WithError_Return500()
        {
            _todoRepositoryMock.Setup(repo => repo.GetTodo(1)).ThrowsAsync(new Exception("Test Exception"));
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.GetTodo(1);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);         
        }

        [Fact]
        public async Task TodoController_PostTodo_Successful_Return201()
        {
            var fakePostTodo = new TodoDbModel { Title = "TestTitle", Description = "TestDescription", Completed = false };
            _todoRepositoryMock.Setup(repo => repo.AddTodo(fakePostTodo)).ReturnsAsync(fakePostTodo);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.PostTodo(fakePostTodo);

            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_PostTodo_Fails_Return500()
        {
            var fakePostTodo = new TodoDbModel { Title = "TestTitle", Description = "TestDescription", Completed = false };
            _todoRepositoryMock.Setup(repo => repo.AddTodo(fakePostTodo)).ThrowsAsync(new Exception("Test Exception"));
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.PostTodo(fakePostTodo);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result.Result);
            var objectResult = result.Result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_DeleteTodo_Found_Return204()
        {
            var fakeTodo = new TodoDbModel { Id = 1 };
            _todoRepositoryMock.Setup(repo => repo.DeleteTodo(fakeTodo.Id)).ReturnsAsync(fakeTodo);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.DeleteTodo(fakeTodo.Id);

            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_DeleteTodo_NotFound_Return404()
        {
            var fakeTodo = new TodoDbModel { Id = 1 };
            _todoRepositoryMock.Setup(repo => repo.DeleteTodo(fakeTodo.Id)).ReturnsAsync((TodoDbModel)null);
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.DeleteTodo(fakeTodo.Id);

            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            var notFoundResult = result as NotFoundResult;
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task TodoController_DeleteTodo_Fails_Return500()
        {
            var fakeTodo = new TodoDbModel { Id = 1 };
            _todoRepositoryMock.Setup(repo => repo.DeleteTodo(fakeTodo.Id)).ThrowsAsync(new Exception("Test Exception"));
            var sut = new TodoController(_todoRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);

            var result = await sut.DeleteTodo(fakeTodo.Id);

            Assert.NotNull(result);
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
