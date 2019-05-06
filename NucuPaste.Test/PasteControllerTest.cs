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

namespace NucuPaste.Test
{
    public class PasteControllerTest
    {
        internal static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            return mockSet;
        }

        [Fact]
        //long pasteId, string fileName, string fileContent
        public void PasteController_GetAction()
        {
            // Arrange
            var mockPasteDbContext = new Mock<PasteDbContext>();
            var mockPasteLogger = new Mock<ILogger<PastesController>>();
            
            var fixture = new Fixture();
            IList<Paste> pastes = new List<Paste>
            {
                fixture.Build<Paste>().Create(),
                fixture.Build<Paste>().Create(),
                fixture.Build<Paste>().Create()
            };
            
            var mockDbSet = GetMockDbSet<Paste>(pastes);
            mockPasteDbContext.Setup(c => c.Pastes).Returns(mockDbSet.Object);

            var pasteController = new PastesController(mockPasteDbContext.Object, mockPasteLogger.Object);
            
            // Act
            List<Paste> results = pasteController.GetPastes().ToList();


            // Assert
            for (var i = 0; i < pastes.Count; i++) {
                Assert.Equal(pastes[i], results[i]);
            }
        }
    }
}
