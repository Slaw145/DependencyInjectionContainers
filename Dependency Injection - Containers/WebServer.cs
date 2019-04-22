using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection___Containers
{
    class WebServer
    {
        static GameServer gameServer;

        static void Main(string[] args)
        {
            var container = new Container();
            container.RegisterAsSingleton<ILoginValidator>(new LoginValidator());
            //container.Register<ILoginValidator, LoginValidator>();
            container.Register<IPasswordValidator, PasswordValidator>();
            container.Register<ICharacterSkillPoints, CharacterSkillPoints>();

            // container.Register<ICharacterRace, ???>();

            var class1 = container.Resolve<ILoginValidator>();
            class1.CountNumberOfCalling();

            var class2 = container.Resolve<ILoginValidator>();
            class2.CountNumberOfCalling();

            container.RegisterType<GameServer>();

            gameServer = container.Resolve<GameServer>();

            gameServer.ResolveInterfaces(container);

            bool ifUserIsLoginIn = LogIn();

            ICharacter createdCharacter = CreateCharacter();

            StartGame(createdCharacter, ifUserIsLoginIn);

            Console.ReadKey();
        }

        static void Shutdown()
        {
            // our container does not implement this
            // container.Dispose();
        }

        static bool LogIn()
        {
            bool ifvalidate = gameServer.RegisterUser("assd12", "adasd123@");

            if (ifvalidate)
            {
                Console.WriteLine("Register user");
            }
            else
            {
                Console.WriteLine("Login or password are incorrect!");
            }

            return ifvalidate;
        }

        static ICharacter CreateCharacter()
        {
            ICharacter character = gameServer.CreateCharacter(new Barbarian());

            Console.WriteLine("Skill points after give out.");

            Console.WriteLine(character.Strength);
            Console.WriteLine(character.Stamina);

            return character;
        }

        static void StartGame(ICharacter character, bool ifvalidate)
        {
            bool ifGameIsStarted = gameServer.StartGame(character, ifvalidate);

            if (ifGameIsStarted)
            {
                Console.WriteLine("Start the game");
            }
            else
            {
                Console.WriteLine("Something went wrong!");
            }
        }
    }
}
