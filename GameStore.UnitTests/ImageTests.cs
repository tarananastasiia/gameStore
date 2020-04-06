using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameStore.Domain.Entities;
using System.Web.Mvc;
using GameStore.WebUI.Controllers;
using GameStore.Domain.Abstract;
using Moq;
using System.Linq;

namespace GameStore.UnitTests
{
    /// <summary>
    /// Summary description for ImageTests
    /// </summary>
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Game с данными изображения
            Game game = new Game
            {
                GameId = 2,
                Name = "Игра2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1"},
                game,
                new Game {GameId = 3, Name = "Игра3"}
            }.AsQueryable());

            // Организация - создание контроллера
            GameController controller = new GameController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> {
                new Game {GameId = 1, Name = "Игра1"},
                new Game {GameId = 2, Name = "Игра2"}
            }.AsQueryable());

            // Организация - создание контроллера
            GameController controller = new GameController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
