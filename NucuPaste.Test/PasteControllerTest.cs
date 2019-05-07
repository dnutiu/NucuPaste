using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Moq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using NucuPaste.Controllers;
using NucuPaste.Data;
using NucuPaste.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace NucuPaste.Test
{
    public class PasteControllerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<ILogger<PastesController>> _testMockLogger;
        private readonly Fixture _testFixtureHelper;

        public PasteControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _testFixtureHelper = new Fixture();
            _testMockLogger = new Mock<ILogger<PastesController>>();
        }

        private void SeedDb(DbContextOptions<PasteDbContext> options, IEnumerable<Paste> pastes)
        {
            using (var context = new PasteDbContext(options))
            {
                foreach (var p in pastes)
                {
                    context.Pastes.Add(p);
                }

                context.SaveChanges();
            }
        }

        [Fact]
        public void PasteController_TestGetAction()
        {
            // Arrange
            List<Paste> results;
            var options = new DbContextOptionsBuilder<PasteDbContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestGetAction").Options;
            var pastes = new List<Paste>
            {
                _testFixtureHelper.Build<Paste>().Create(),
                _testFixtureHelper.Build<Paste>().Create(),
                _testFixtureHelper.Build<Paste>().Create()
            };

            SeedDb(options, pastes);

            // Act
            using (var context = new PasteDbContext(options))
            {
                var pasteController = new PastesController(context, _testMockLogger.Object);
                results = pasteController.GetPastes().ToList();
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
            var options = new DbContextOptionsBuilder<PasteDbContext>()
                .UseInMemoryDatabase(databaseName: "PasteController_TestGetByIdAction").Options;
            var pastes = new List<Paste>
            {
                _testFixtureHelper.Create<Paste>(),
                searchedPaste
            };
            SeedDb(options, pastes);
            
            // Act
            using (var context = new PasteDbContext(options))
            {
                var pasteController = new PastesController(context, _testMockLogger.Object);
                var res = await pasteController.GetPaste(1) as OkObjectResult;
                
                Debug.Assert(res != null, nameof(res) + " != null");
                result = res.Value as Paste;
            }
            
            // Assert
            Assert.Equal(searchedPaste, result, Paste.EqualityComparer);
        }
    }
}