using Dependency_Injection___Containers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DepedencyInjectionContainersTests
{
    class GameServerClassTest
    {
        GameServer gameserver;

        Mock <ICharacterRace> characterRaceMock;
        Mock <ICharacter> characterMock;
        ICharacter character;

        [SetUp]
        public void TestSetup()
        {
            characterRaceMock = new Mock<ICharacterRace>();
            characterMock = new Mock<ICharacter>();

            character = characterMock.Object;

            var container = new Container();
            container.Register<ILoginValidator, LoginValidator>();
            container.Register<IPasswordValidator, PasswordValidator>();
            container.Register<ICharacterSkillPoints, CharacterSkillPoints>();

            // container.Register<ICharacterRace, ???>();

            container.RegisterType<GameServer>();

            gameserver = container.Resolve<GameServer>();

            gameserver.ResolveInterfaces(container);
        }

        [Test]
        public void test_race_attributes()
        {
            characterRaceMock.Setup(x => x.CreateCharacterRace(It.IsAny<ICharacterRace>())).Returns(() => characterRaceMock.Object);

            Assert.IsInstanceOf(typeof(Mock), characterRaceMock);
        }

        [Test]
        public void start_the_game_test_correctly()
        {
            bool ifvalidate = gameserver.RegisterUser("assd12", "adasd123@");

            bool ifGameIsStarted = gameserver.StartGame(character, ifvalidate);

            Assert.IsTrue(ifGameIsStarted);
        }

        [Test]
        public void start_the_game_test_not_correctly_with_bad_login_and_password()
        {
            bool ifvalidate = gameserver.RegisterUser("assd", "adasd12");

            bool ifGameIsStarted = gameserver.StartGame(character, ifvalidate);

            Assert.IsFalse(ifGameIsStarted);
        }

        [Test]
        public void start_the_game_test_not_correctly_without_created_character()
        {
            bool ifvalidate = gameserver.RegisterUser("assd12", "adasd123@");

            bool ifGameIsStarted = gameserver.StartGame(null, ifvalidate);

            Assert.IsFalse(ifGameIsStarted);
        }
    }
}
