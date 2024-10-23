using Microsoft.Playwright;
using static Microsoft.Playwright.Playwright;

namespace DemoQA
{
    public class MainPageTest : PageTest
    {
        private IPage Page {get; set;}

        [SetUp]
        public async Task SetUp(){
            var playwright = await CreateAsync();
            playwright.Selectors.SetTestIdAttribute("aria-label");
            var browser =  await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{
                Headless= false
            });
            Page = await browser.NewPageAsync();
            await Page.GotoAsync("https://demoqa.com");
        }

        [Test]
        public async Task HaveCardsVisibleAsync()
        {
            await Expect(Page.GetByText("Elements")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Forms")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Alerts, Frame & Windows")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Widgets")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Interactions")).ToBeVisibleAsync();
            await Expect(Page.GetByText("Book Store Application")).ToBeVisibleAsync();
        }

        [TestCase("Elements", "elements")]
        [TestCase("Forms", "forms")]
        [TestCase("Alerts, Frame & Windows", "alertsWindows")]
        [TestCase("Widgets", "widgets")]
        [TestCase("Interactions", "interaction")]
        [TestCase("Book Store Application", "books")]
        [Parallelizable(ParallelScope.Self)]
        public async Task CheckUrlForCards(string element, string url)
        {
            await Page.ClickAsync($"text={element}");
            await Expect(Page).ToHaveURLAsync($"https://demoqa.com/{url}");
        }

        [TearDown]
        public async Task TearDown()
        {
            await Page.CloseAsync();
        }
    }
}