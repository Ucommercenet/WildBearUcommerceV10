using Microsoft.AspNetCore.Mvc;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleTestController : ControllerBase
    {
        [HttpGet("AskQuestionGet")]
        public int AskQuestionGet()
        {
            return 42;
        }

        [HttpGet("BestAnswerGet")]
        public ActionResult<string> BestAnswerGet(int guess)
        {
            return guess == 42 ? "Correct" : "Incorrect";
        }
    }
}