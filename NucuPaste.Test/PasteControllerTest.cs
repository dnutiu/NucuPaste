using System;
using System.Collections.Generic;
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
using Xunit.Abstractions;

namespace NucuPaste.Test
{
    public class PasteControllerTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PasteControllerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        //long pasteId, string fileName, string fileContent
        public void PasteController_GetAction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PasteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;
            var mockPasteLogger = new Mock<ILogger<PastesController>>();
            
            var fixture = new Fixture();
            var pastes = new List<Paste>
            {
                fixture.Build<Paste>().Create(),
                fixture.Build<Paste>().Create(),
                fixture.Build<Paste>().Create()
            };

            
            // Act
            List<Paste> results;
            using (var context = new PasteDbContext(options))
            {
                foreach (var p in pastes)
                {
                    context.Pastes.Add(p);
                }

                context.SaveChanges();
                
                var pasteController = new PastesController(context, mockPasteLogger.Object);
                results = pasteController.GetPastes().ToList();
            }

            // Assert
            for (var i = 0; i < pastes.Count; i++) {
                Assert.Equal(pastes[i], results[i]);
            }
        }
    }
}
