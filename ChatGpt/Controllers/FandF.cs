using ChatGpt.Code;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace ChatGpt.Controllers
{
    public class FandF : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOpenAIProxy _chatOpenAI;
        private const string BeginPrompt = @"
            You are going to write a fast and furious script. 
            You only can only include two characters: Dominic Toretto and Paul Walker. 
            In the script you should structure it like the following with the characters name in 
            square brackets then their speech, at the end of their line the next characters line should 
            start on a new line. You can also give them the stage directions by putting the directions inside *s. 
            You can only use the following stage directions: *gets in car*, *drives car*, *leaves car*, *crashes car*, *shoots gun*. 
            Do not use any other stage directions but the ones listed. They must race cars, and Paul Walker must get mildly injured. 
            Dom must massively overreact to the injury. Also they must talk about family at least once in the script. 
            Paul Walker must also mention his favourite anime, which is Death Note, at least once. 
            You will be provided a scenario to write about. Here is an example of a script:

            [Dominic Toretto]: Hello Paul, how are you?
            [Paul Walker]: I am good thank you, how are you?
            [Dominic Toretto]: Shall we go for a drive?
            [Paul Walker]: Yes I love to drive *gets in car*
            [Dominic Toretto]: *gets in car*
            [Paul Walker]: lets race to the bar and back *drives car*
            [Dominic Toretto]: oh you're on, i bet my family I can beat you *drives car*
            [Paul Walker]: *crashes car* aaaaah my ankle
            [Dominic Toretto]: *leaves car* oh no my friend is hurt
            [Paul Walker]: ouchie, this is just like in death note which i watched with my family

            now here is the prompt of what I should like you to write about:";

        public FandF(ILogger<HomeController> logger)
        {
            _logger = logger;
            _chatOpenAI = new OpenAIProxy
                (
                    apiKey: "", //Put your keys in here
                    organizationId: ""
                );
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string[]> CallChatGtp(string msg)
        {
            string converstaion = BeginPrompt + "\n" + msg;
            List<string> Answers = new List<string>();

            //CHAT GPT STUFF - COSTs
            var results = await _chatOpenAI.SendChatMessage(converstaion, 2500);
            foreach (var item in results)
            {
                Answers.Add(item.Content);
            }

            //Answers.Add(@"""[Paul Walker]: Hey Dom, I'm craving some sushi. How about we go to Yo Sushi?

            //            [Dominic Toretto]: Sounds good to me, Paul. Let's satisfy our hunger for speed and sushi.

            //            [Paul Walker]: *gets in car* Buckle up, Dom. We're in for a fast and furious ride.

            //            [Dominic Toretto]: *gets in car* You know, Paul, racing to Yo Sushi reminds me of the importance of family.

            //            [Paul Walker]: Absolutely, Dom. Family is everything. Speaking of which, have you ever watched Death Note?

            //            [Dominic Toretto]: Can't say I have, Paul. What's it about?

            //            [Paul Walker]: It's an incredible anime about a high school student who discovers a supernatural notebook that grants him the power to kill anyone by writing their name in it. It's a thrilling battle of wits and justice.

            //            [Dominic Toretto]: Sounds intense, Paul. We'll have to watch it together with our family sometime.

            //            [Paul Walker]: Definitely, Dom. Now, let's put the pedal to the metal and race to Yo Sushi. *drives car*

            //            [Dominic Toretto]: You're on, Paul. But remember, safety first. We don't want any injuries.

            //            [Paul Walker]: Don't worry, Dom. I'll keep it under control. *drives car*

            //            [Dominic Toretto]: *drives car* I bet my family's secret recipe for tuna rolls that I can beat you.

            //            [Paul Walker]: Challenge accepted, Dom. Let's see who gets to Yo Sushi first. *drives car*

            //            *Both cars speed down the road, pushing their limits.*

            //            [Paul Walker]: *crashes car* Ouch! My arm!

            //            [Dominic Toretto]: *leaves car* Paul, are you okay? We need to get you some help.

            //            [Paul Walker]: It's just a minor injury, Dom. Nothing compared to the action in Death Note. *laughs*

            //            [Dominic Toretto]: *helps Paul out of the car* You scared me there, Paul. Family means everything, and I can't bear to see you hurt.

            //            [Paul Walker]: Thanks, Dom. I appreciate your concern. Let's get some sushi and enjoy our time together, just like a family would.

            //            [Dominic Toretto]: Absolutely, Paul. We'll make it a memorable meal. *they walk towards Yo Sushi*""
            //            ");

            string[] response = Answers.ToArray();
            return response;
        }
    }
}
