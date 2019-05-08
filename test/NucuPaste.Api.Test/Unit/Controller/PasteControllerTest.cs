using System.Collections.Generic;
using System.Diagnostics;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NucuPaste.Api.Controllers;
using NucuPaste.Api.Data;
using NucuPaste.Api.Models;
using NucuPaste.Api.Services;
using Xunit;
using Xunit.Abstractions;

namespace NucuPaste.Api.Test.Unit.Controller
{
    public class PasteControllerTest
    {
        private readonly Mock<ILogger<PastesController>> _testMockLogger;
        private readonly Fixture _testFixtureHelper;

        public PasteControllerTest()
        {
            _testFixtureHelper = new Fixture();
            _testMockLogger = new Mock<ILogger<PastesController>>();
        }

        private static void SeedDb(DbContextOptions<NucuPasteContext> options, IEnumerable<Paste> pastes)
        {
            using (var context = new NucuPasteContext(options))
            {
                foreach (var p in pastes)
                {
                    context.Pastes.Add(p);
                }

                context.SaveChanges();
            }
        }

        [Fact]
        public async void PasteController_TestGetAction()
        {
            // Arrange
            List<Paste> results;
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestGetAction").Options;
            var pastes = new List<Paste>
            {
                _testFixtureHelper.Build<Paste>().Create(),
                _testFixtureHelper.Build<Paste>().Create(),
                _testFixtureHelper.Build<Paste>().Create()
            };

            SeedDb(options, pastes);

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                results = await pasteController.GetPastes();
            }

            // Assert
            for (var i = 0; i < pastes.Count; i++)
            {
                Assert.Equal(pastes[i], results[i], Paste.EqualityComparer);
            }
        }

        [Fact]
        public async void PasteController_TestGetByIdAction_ResultFound()
        {
            // Arrange
            Paste result;
            var searchedPaste = _testFixtureHelper.Build<Paste>().With(p => p.Id, 1).Create();
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestGetByIdAction").Options;
            var pastes = new List<Paste>
            {
                _testFixtureHelper.Create<Paste>(),
                searchedPaste
            };
            SeedDb(options, pastes);

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                var res = await pasteController.GetPaste(1) as OkObjectResult;

                Debug.Assert(res != null, nameof(res) + " != null");
                result = res.Value as Paste;
            }

            // Assert
            Assert.Equal(searchedPaste, result, Paste.EqualityComparer);
        }

        [Fact]
        public async void PasteController_TestGetByIdAction_ResultNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestGetByIdAction").Options;
            var pastes = new List<Paste>
            {
                _testFixtureHelper.Create<Paste>(),
                _testFixtureHelper.Create<Paste>(),
                _testFixtureHelper.Create<Paste>(),
            };
            SeedDb(options, pastes);
            NotFoundResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.GetPaste(1111111) as NotFoundResult;
            }

            // Assert
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void PasteController_TestDeleteAction_Success()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestDeleteAction_Success").Options;
            SeedDb(options, new Paste[] {_testFixtureHelper.Build<Paste>()
                .With(p => p.Id, 1).Create()});
            NoContentResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.DeletePaste(1) as NoContentResult;
            }

            // Assert
            using (var context = new NucuPasteContext(options))
            {
                Debug.Assert(result != null, nameof(result) + " != null");
                Assert.Equal(204, result.StatusCode);
                var res = await context.Pastes.FindAsync((long)1);
                Assert.Null(res);
            }
        }

        [Fact]
        public async void PasteController_TestDeleteAction_NotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestDeleteAction_NotFound").Options;
            SeedDb(options, new Paste[] {_testFixtureHelper.Build<Paste>()
                .With(p => p.Id, 1).Create()});
            NotFoundResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.DeletePaste(1111111) as NotFoundResult;
            }

            // Assert
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void PasteController_TestPostAction_Success()
        {
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestPostAction_Success").Options;
            SeedDb(options, new Paste[] { });
            var newPaste = _testFixtureHelper.Create<Paste>();
            CreatedAtActionResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.PostPaste(newPaste, ApiVersion.Default) as CreatedAtActionResult;
            }

            // Assert
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.Equal(newPaste, result.Value as Paste, Paste.EqualityComparer);
        }

        [Fact]
        public async void PasteController_TestPutAction_Success()
        {
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestPutAction_Success").Options;
            SeedDb(options, new Paste[]
                {_testFixtureHelper.Build<Paste>().With(p => p.Id, 1).Create()});
            var newPaste = _testFixtureHelper.Build<Paste>().With(p => p.Id, 1).Create();
            OkObjectResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.PutPaste(newPaste.Id, newPaste) as OkObjectResult;
            }

            // Assert
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.Equal(newPaste, result.Value as Paste, Paste.EqualityComparer);
        }

        [Fact]
        public async void PasteController_TestPutAction_Failure()
        {
            var options = new DbContextOptionsBuilder<NucuPasteContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestPutAction_Failure").Options;
            SeedDb(options, new Paste[]
                {_testFixtureHelper.Build<Paste>().With(p => p.Id, 1).Create()});
            var newPaste = _testFixtureHelper.Build<Paste>().With(p => p.Id, 2).Create();
            NotFoundResult result;

            // Act
            using (var context = new NucuPasteContext(options))
            {
                var pasteController = new PastesController(_testMockLogger.Object, new PasteService(context));
                result = await pasteController.PutPaste(2, newPaste) as NotFoundResult;
            }

            // Assert
            Debug.Assert(result != null, nameof(result) + " != null");
            Assert.Equal(404, result.StatusCode);
        }
    }
}