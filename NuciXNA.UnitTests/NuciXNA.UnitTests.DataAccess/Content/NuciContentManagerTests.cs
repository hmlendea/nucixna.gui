using System;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using Moq;
using NUnit.Framework;

using NuciXNA.DataAccess.Content;

namespace NuciXNA.UnitTests.DataAccess.Content
{
    public class ContentManagerTests
    {
        Mock<IContentLoader> pipelineContentLoaderMock;
        Mock<IContentLoader> plainFileContentLoaderMock;

        [SetUp]
        public void SetUp()
        {
            pipelineContentLoaderMock = new Mock<IContentLoader>();
            plainFileContentLoaderMock = new Mock<IContentLoader>();

            NuciContentManager.Instance.LoadContent(
                pipelineContentLoaderMock.Object,
                plainFileContentLoaderMock.Object);
        }

        [Test]
        public void LoadSoundEffect_PipelineContentExists_PlainFileExists_ReturnsPipelineContent()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()));
            
            plainFileContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()));
            
            Assert.DoesNotThrow(() => NuciContentManager.Instance.LoadSoundEffect("testSoundEffect"));
        }

        [Test]
        public void LoadSoundEffect_PipelineContentExists_PlainFileDoesntExist_ReturnsPipelineContent()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()));
            
            plainFileContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()));
            
            Assert.DoesNotThrow(() => NuciContentManager.Instance.LoadSoundEffect("testSoundEffect"));
        }

        [Test]
        public void LoadSoundEffect_PipelineContentDoesntExist_PlainFileExists_ReturnsPlainFileContent()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()))
                .Throws<Exception>();
            
            plainFileContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()));
            
            Assert.DoesNotThrow(() => NuciContentManager.Instance.LoadSoundEffect("testSoundEffect"));
        }

        [Test]
        public void LoadSoundEffect_PipelineContentDoesntExist_PlainFileDoesntExist_ThrowsException()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()))
                .Throws<Exception>();
            
            plainFileContentLoaderMock
                .Setup(x => x.LoadSoundEffect(It.IsAny<string>()))
                .Throws<Exception>();
            
            Assert.Throws<Exception>(() => NuciContentManager.Instance.LoadSoundEffect("testSoundEffect"));
        }

        [Test]
        public void LoadSpriteFont_PipelineContentExists_PlainFileDoesntExist_ReturnsPipelineContent()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSpriteFont(It.IsAny<string>()));
            
            Assert.DoesNotThrow(() => NuciContentManager.Instance.LoadSpriteFont("testSpriteFont"));
        }

        [Test]
        public void LoadSpriteFont_PipelineContentDoesntExist_ThrowsException()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadSpriteFont(It.IsAny<string>()))
                .Throws<Exception>();
            
            Assert.Throws<Exception>(() => NuciContentManager.Instance.LoadSpriteFont("testSpriteFont"));
        }

        [Test]
        public void LoadTexture2D_PipelineContentDoesntExist_PlainFileExists_ThrowsException()
        {
            pipelineContentLoaderMock
                .Setup(x => x.LoadTexture2D(It.IsAny<string>()))
                .Throws<Exception>();
            
            plainFileContentLoaderMock
                .Setup(x => x.LoadTexture2D(It.IsAny<string>()))
                .Throws<Exception>();
            
            Assert.Throws<Exception>(() => NuciContentManager.Instance.LoadTexture2D("testTexture2D"));
        }
    }
}
