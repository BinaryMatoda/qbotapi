using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using qbotapi.Controllers.ServiceInterfaces;
using qbotapi.Resources;

namespace qbotapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class chgkController : ControllerBase
    {
        private readonly IChgkHttpService _service;
        public chgkController(IChgkHttpService service)
        {
            _service = service;
        }
        // GET: api/chgk/type/:type/complexity/:comp/limit/:limit/
        //[HttpGet("/{type}")]
        [HttpGet]
        [Route("type/{type}/complexity/{complexity}/limit/{limit}")]
        public async Task<IEnumerable<QuestionResource>> GetQuestion(int type, int complexity, int limit)
        {
            var res = await _service.OnGet(type, complexity, limit);
            var contents = await res.Content.ReadAsStringAsync();
            var questions = GetQuestionsFromResponseContent(contents);
            return questions;
        }

        private IEnumerable<QuestionResource> GetQuestionsFromResponseContent(string content)
        {
            string divBegin = @"<div class='random_question'>";
            string divEnd = @"</div>";
            string requiredPhrase = "Вопрос";

            var strings = content.Split(divBegin);
            var questions = new List<QuestionResource>();
            foreach (var item in strings)
            {
                QuestionResource question = ConvertStringToQuestion(item.Contains(requiredPhrase) ? divBegin + item + divEnd : item, divBegin, divEnd);
                if (question.answer.Length > 0 && question.question.Length > 0)
                {
                    questions.Add(question);
                }
            }
            return questions;
        }

        private string replaceEncodedSymbols(string text)
        {
            var rules = new Dictionary<string, string>()
            {
                { "<br/>","\\n" },
                { "<br />","\\n" },
                { "<br>","\\n" },
                { "&nbsp;"," " },
                { "&mdash;","-" }
            };
            foreach (var pattern in rules.Keys)
            {
                text = Replace(text, pattern, rules[pattern]);
            }
            return text;
        }

        private string getTextBetweenTags(string text, string openTag, string closeTag)
        {
            if (string.IsNullOrEmpty(closeTag)) closeTag = openTag;
            var index1 = text.IndexOf(openTag);
            if (index1 >= 0)
            {
                var index2 = text.IndexOf(closeTag, index1 + 1);
                if (index2 >= 0)
                {
                    var between = text.Substring(index1 + openTag.Length, index2 - index1 - openTag.Length);
                    between = Regex.Replace(between, @"<br/>", @"\n");
                    between = Regex.Replace(between, @"<br />", @"\n");
                    between = Regex.Replace(between, @"<br>", @"\n");
                    between = Regex.Replace(between, @"&nbsp;", @" ");
                    between = Regex.Replace(between, @"&mdash;", @"-");
                    return between;
                }
            }
            return "";
        }

        private QuestionResource ConvertStringToQuestion(string html, string divBegin, string divEnd)
        {
            var index1 = html.IndexOf(divBegin);
            //console.log(index1)
            //console.log(html.indexOf('random_question'))


            var question = getTextBetweenTags(html, divBegin, divEnd);
            var title = Replace(getTextBetweenTags(getTextBetweenTags(question, "<p>", "</p>"), ">", "<"), @"\\n", " ");//.replace(/\n / g, " ");
            var text = Replace(getTextBetweenTags(question, "</strong>", "<p>"), @"\\n", " "); ;//.replace(/\n / g, " ");

            var img = getTextBetweenTags(question, "<img", ">");

            var answer = getTextBetweenTags(question, "<strong>Ответ:</strong>", "</p>");
            var answer2 = getTextBetweenTags(question, "<strong>Зачёт:</strong>", "</p>");
            var _author = getTextBetweenTags(question, "<strong>Автор:</strong>", "</p>");
            var source = Replace(getTextBetweenTags(question, "<strong>Источник(и):</strong>", "</p>"), @"\\n", " ");//.replace(/\n / g, " ");
            var comment = Replace(getTextBetweenTags(question, "<strong>Комментарий:</strong>", "</p>"), @"\\n", " ");//.replace(/\n / g, " ");

            var tmp = getTextBetweenTags(_author, ">", "<");
            var author = tmp.Length == 0 ? _author : tmp;

            var result = new QuestionResource()
            {
                answer = answer,
                title = title,
                answer2 = answer2,
                author = author,
                comment = comment,
                imageLink = img.Length == 0 ? "" : "<img " + img + ">",
                question = text,
                source = source
            };
            return result;
        }

        private string Replace(string text, string pattern, string replacementText)
        {
            return Regex.Replace(text, @pattern, @replacementText);
        }
    }
}
