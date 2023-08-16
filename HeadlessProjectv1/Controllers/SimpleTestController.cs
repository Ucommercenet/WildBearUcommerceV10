using Microsoft.AspNetCore.Mvc;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleTestController : ControllerBase
    {
        public int AskQuestionGet()
        {
            return 42;
        }

        //[HttpGet("AskQuestionGet2")]
        //public int AskQuestionGet2()
        //{
        //    return 85;
        //}

        //public ActionResult<string> BestAnswerGet(int guess)
        //{
        //    if (guess == 42)
        //    {
        //        return "Correct";
        //    }

        //    return this.BadRequest();
        //}
    }
}